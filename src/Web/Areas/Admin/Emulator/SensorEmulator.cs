using Fleck;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Timers;
using System.Web;
using Web.Helpers;
using WebSocketSharp;

namespace Web.Areas.Admin.Emulator
{
    public class SensorEmulator
    {
        private string _pairingRoute => "pairing";

        private readonly string _sensorGuid;
        private readonly string _baseUrl;

        private WebServer _webServer;
        private WebSocket _webSocket;
        private Timer _timer;

        public SensorEmulator(string baseUrl)
        {
            _sensorGuid = Guid.NewGuid().ToString();
            _baseUrl = baseUrl;
        }

        public SensorState State { get; private set; }

        public bool IsPowerOn { get; private set; }

        public void PowerOn()
        {
            RestoreState();
            if (!State.IsMated)
            {
                InitializeWebServer();
            }
            else
            {
                InitializeWebSockets();
            }
            IsPowerOn = true;
        }

        public void PowerOff()
        {
            if (_webServer != null && _webServer.IsOn())
            {
                _webServer.Stop();
            }
            if (_webSocket != null && _webSocket.ReadyState != WebSocketState.Closed)
            {
                _webSocket.Close();
            }
            _webServer = null;
            _webSocket = null;
            State = null;
            IsPowerOn = false;
        }

        #region Private

        private void InitializeWebServer()
        {
            _webServer = new WebServer(_baseUrl, new List<string>() {
                    _pairingRoute
                 });
            _webServer.OnRequest += HandleRequest;
            _webServer.Run();
        }

        private void InitializeWebSockets()
        {
            _timer = new Timer(TimeSpan.FromMinutes(1).TotalMilliseconds);
            _timer.Elapsed += (e, arg) =>
            {
                if (_webSocket != null && _webSocket.ReadyState == WebSocketState.Closed)
                {
                    StartWebSocketConnection();
                }
            };
            StartWebSocketConnection();
        }

        private void RestoreState()
        {
            State = CacheHelper.Get<SensorState>(_sensorGuid) ?? new SensorState();
            var geolocation = GetGeolocation();
            State.Latitude = geolocation.latitude;
            State.Longitude = geolocation.longitude;
        }

        private void UpdateState(SetSensorStateModel newStateModel)
        {
            State.IsActive = newStateModel.IsActive;
            State.TrackingKey = newStateModel.TrackingKey;
            State.Type = newStateModel.Type;
            State.WebServerIP = newStateModel.WebServerUrl;
            CacheHelper.AddOrUpdate(_sensorGuid, State);
        }

        private void HandleRequest(HttpListenerContext ctx)
        {
            if (ctx.Request.RawUrl.Contains(_pairingRoute) && new HttpMethod(ctx.Request.HttpMethod) == HttpMethod.Post)
            {
                var state = WebServer.GetRequestBody<SetSensorStateModel>(ctx.Request);
                UpdateState(state);
                WebServer.SetReponse(ctx, new HttpResponseMessage(HttpStatusCode.OK));
                _webServer.Stop();
                InitializeWebSockets();
            }
        }

        #region Helpers

        private (double latitude, double longitude) GetGeolocation()
        {
            return (0, 0);
        }


        private void StartWebSocketConnection()
        {
            _webSocket = new WebSocket(State.WebSocketPath);
            _webSocket.OnMessage += (sender, e) => Console.WriteLine("Laputa says: " + e.Data);
            _webSocket.OnError += (sender, e) => _webSocket.Close();
            _webSocket.Connect();
            _webSocket.Send("");
        }
    }
    #endregion

    #endregion
}