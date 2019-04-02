using System;
using System.Collections.Generic;
using System.Linq;
using WebSocketSharp;
using System.Web;
using Web.SensorActions.Output;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Web.Data.Models;
using Web.Data;
using Web.Enum;
using AutoMapper;
using Web.SensorActions.Input;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Timers;
using Microsoft.AspNet.SignalR;
using Web.Hub;

namespace Web.Helpers
{
    public static class SensorWebSocketHelper
    {
        private static Dictionary<int, WebSocket> Connections { get; set; } = new Dictionary<int, WebSocket>();
        private static Dictionary<int, Timer> Timers { get; set; } = new Dictionary<int, Timer>();
        private static readonly IMapper _mapper = new Mapper(new MapperConfiguration(x =>
        {
            x.CreateMap<PushReadingsActionPayload, Reading>();
        }));

        public static async Task ConnectAllSensorsAsync()
        {
            using (var _context = new DataContext())
            {
                var sensors = await _context.Sensors.Where(f => !f.IsDeleted).ToListAsync();
                foreach (var sensor in sensors)
                {
                    try
                    {
                        ConnectSensor(sensor);
                    }
                    catch (InvalidOperationException)
                    {

                    }
                }
            }
        }

        public static void ConnectSensor(Sensor sensor)
        {
            WebSocket ws = null; ;
            if (Connections.ContainsKey(sensor.Id))
            {
                ws = Connections[sensor.Id];
                if (ws != null)
                {
                    if (ws.ReadyState == WebSocketState.Open)
                    {
                        LoggerHelper.Log.Info("Попытка подключить уже подключенный датчик!");
                        return;
                    }
                    ws.Close();
                }
            }
            ws = new WebSocket($"ws://{sensor.IPAddress}");
            ws.OnMessage += async (sender, e) =>
           {
               dynamic json = JsonConvert.DeserializeObject(e.Data);
               if (sensor.Type == SensorType.Static)
               {
                   if (json.type == InputSensorActionType.PushReadings)
                   {
                       var reading = _mapper.Map<PushReadingsActionPayload, Reading>((json as PushReadingsAction).Payload);
                       reading.SensorId = sensor.Id;
                       await DatabaseHelper.AddReadingAsync(reading);
                       var pollutionLevel = PollutionHelper.GetPollutionLevel(sensor.Id);
                       DispatchHelper.DispatchReadingsForStaticSensor(sensor.Id, pollutionLevel, reading);
                   }
               }
               else if (sensor.Type == SensorType.Portable)
               {
                   if (json.type == InputSensorActionType.PushReadings)
                   {
                       var reading = _mapper.Map<PushReadingsActionPayload, Reading>((json as PushReadingsAction).Payload);
                       reading.SensorId = sensor.Id;
                       DispatchHelper.DispatchReadingsForPortableSensor(sensor.Id, reading);
                   }
                   if (json.type == InputSensorActionType.PushCoordinates)
                   {
                       var coordinates = (json as PushCoordinatesAction).Payload;
                       DispatchHelper.DispatchCoordinatesForPortableSensor(sensor.Id, coordinates.Latitude, coordinates.Longitude);
                   }
               }
           };
            ws.OnClose += (sender, e) =>
            {
                LoggerHelper.Log.Info($"Датчик с id {sensor.Id} был отключен!");
            };
            ws.OnError += (sender, e) =>
            {
                LoggerHelper.Log.Error($"Произошла ошибка при работе с датчиком с id {sensor.Id}", e.Exception);

            };
            ws.Connect();
            if (ws.ReadyState != WebSocketState.Open)
            {
                throw new InvalidOperationException("Датчик не включен");
            }
            Connections[sensor.Id] = ws;
            TriggerChangeState(sensor);
        }

        public static void DisconnectSensor(int id)
        {
            DisconnectWebSocket(id);
            StopTimer(id);
        }

        public static bool IsConnected(int id)
        {
            if (!Connections.ContainsKey(id))
            {
                return false;
            }
            var webSocket = Connections[id];
            if (webSocket == null)
            {
                return false;
            }
            if (webSocket.ReadyState == WebSocketState.Open)
            {
                return true;
            }
            return false;
        }


        public static void TriggerChangeState(Sensor sensor)
        {
            if (Timers.ContainsKey(sensor.Id))
            {
                Timers[sensor.Id].Stop();
            }
            if (!sensor.IsActive || sensor.IsDeleted)
            {
                Timers[sensor.Id] = null;
                return;
            }
            Timers[sensor.Id] = new Timer();
            if (sensor.Type == SensorType.Portable)
            {
                Timers[sensor.Id].Interval = 500;
            }
            else if (sensor.Type == SensorType.Static)
            {
                Timers[sensor.Id].Interval = 10000;
            }
            Timers[sensor.Id].Elapsed += (sender, e) =>
            {
                if (Connections.ContainsKey(sensor.Id))
                {
                    Connections[sensor.Id].Send(SerializeJson(new PullReadingsAction(new PullReadingsActionPayload())));
                    if (sensor.Type == SensorType.Portable)
                    {
                        Connections[sensor.Id].Send(SerializeJson(new PullCoordinatesAction(new PullCoordinatesActionPayload())));
                    }
                }
            };
            Timers[sensor.Id].Start();
        }

        private static void DisconnectWebSocket(int id)
        {
            if (!Connections.ContainsKey(id))
            {
                return;
            }
            var webSocket = Connections[id];
            if (webSocket == null)
            {
                return;
            }
            if (webSocket.ReadyState != WebSocketState.Closed)
            {
                webSocket.Close();
            }
            Connections.Remove(id);
        }

        private static void StopTimer(int id)
        {
            if (!Timers.ContainsKey(id))
            {
                return;
            }
            var timer = Timers[id];
            if (timer == null)
            {
                return;
            }
            if (timer.Enabled)
            {
                timer.Stop();
            }
            Timers.Remove(id);
        }

        private static string SerializeJson(object json)
        {
            return JsonConvert.SerializeObject(json, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }
    }
}