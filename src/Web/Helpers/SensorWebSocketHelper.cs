using System;
using System.Collections.Generic;
using System.Linq;
using WebSocketSharp;
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
using Web.Areas.Admin.Helpers;
using Web.Areas.PWA.Helpers;

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
                var sensors = await DatabaseHelper.GetSensorsAsync();
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
               //TODO: check sensor state here
               dynamic json = JsonConvert.DeserializeObject(e.Data);
               var sensorState = await DatabaseHelper.GetSensorByIdAsync(sensor.Id);
               if (sensorState is StaticSensor)
               {
                   var staticSensorState = (StaticSensor)sensorState;
                   if (json["type"] == InputSensorActionType.PushReadings)
                   {
                       var payload = JsonConvert.DeserializeObject<PushReadingsActionPayload>(JsonConvert.SerializeObject(json["payload"]));
                       var reading = _mapper.Map<PushReadingsActionPayload, Reading>(payload);
                       reading.SensorId = sensor.Id;
                       await DatabaseHelper.AddReadingAsync(reading);
                       if (staticSensorState.IsAvailable())
                       {
                           await SensorCacheHelper.UpdateSensorCacheWithReadingAsync(reading);
                           var pollutionLevel = await SensorCacheHelper.GetPollutionLevelAsync(sensor.Id);
                           PWADispatchHelper.DispatchReadingsForStaticSensor(sensor.Id, pollutionLevel, reading);
                       }
                       AdminDispatchHelper.DispatchReadingsForStaticSensor(sensor.Id, reading);
                   }
               }
               else if (sensorState is PortableSensor)
               {
                   if (json.type == InputSensorActionType.PushReadings)
                   {
                       var payload = JsonConvert.DeserializeObject<PushReadingsActionPayload>(JsonConvert.SerializeObject(json["payload"]));
                       var reading = _mapper.Map<PushReadingsActionPayload, Reading>(payload);
                       reading.SensorId = sensor.Id;
                       AdminDispatchHelper.DispatchReadingsForPortableSensor(sensor.Id, reading);
                   }
                   if (json.type == InputSensorActionType.PushCoordinates)
                   {
                       var payload = JsonConvert.DeserializeObject<PushCoordinatesActionPayload>(JsonConvert.SerializeObject(json["payload"]));
                       AdminDispatchHelper.DispatchCoordinatesForPortableSensor(sensor.Id, payload.Latitude, payload.Longitude);
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

        public static void DisconnectAllSensors()
        {
            var keysC = Connections.Select(f => f.Key).ToList();
            foreach (var key in keysC)
            {
                DisconnectWebSocket(key);
            }
            var keysT = Timers.Select(f => f.Key).ToList();
            foreach (var key in keysC)
            {
                StopTimer(key);
            }
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
            Timers[sensor.Id] = new Timer();
            if (!sensor.IsActive || sensor.IsDeleted)
            {
                return;
            }
            if (sensor is PortableSensor)
            {
                Timers[sensor.Id].Interval = 500;
            }
            else if (sensor is StaticSensor)
            {
                Timers[sensor.Id].Interval = 10000;
            }
            Timers[sensor.Id].Elapsed += (sender, e) =>
            {
                if (Connections.ContainsKey(sensor.Id))
                {
                    Connections[sensor.Id].Send(SerializeJson(new PullReadingsAction(new PullReadingsActionPayload())));
                    if (sensor is PortableSensor)
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