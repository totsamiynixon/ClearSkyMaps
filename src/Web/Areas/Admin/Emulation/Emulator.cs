using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;
using Web.Data;
using Web.Data.Models;
using Z.EntityFramework.Plus;

namespace Web.Areas.Admin.Emulation
{
    public static class Emulator
    {
        public static bool IsEmulationEnabled { get; private set; }
        private static Random _emulatorRandom = new Random();
        private static bool _firstInit = true;
        private static List<string> _guids { get; set; }
        public static List<SensorEmulator> Devices { get; private set; }


        public static async Task RunEmulationAsync()
        {
            if (!IsEmulationEnabled)
            {
                IsEmulationEnabled = true;
            }
            if (_firstInit)
            {
                await SeedSensorsAsync();
                _firstInit = false;
            }
        }

        public static void StopEmulation()
        {
            IsEmulationEnabled = false;
        }

        private static async Task SeedSensorsAsync()
        {
            _guids = new List<string>();
            Devices = new List<SensorEmulator>();
            using (var context = new DataContext())
            {
                await context.Sensors.Where(f => f.Id > 0).DeleteAsync();
                var iterations = _emulatorRandom.Next(0, 20);
                for (int i = 0; i < iterations; i++)
                {
                    var guid = Guid.NewGuid().ToString();
                    _guids.Add(guid);
                    var fakeSensor = GetFakeSensor(guid);
                    context.Sensors.Add(fakeSensor.sensor);
                    Devices.Add(fakeSensor.emulator);
                }
                await context.SaveChangesAsync();
            }
        }

        private static (SensorEmulator emulator, Sensor sensor) GetFakeSensor(string guid)
        {
            var sensorEmulator = new SensorEmulator(GetLocalIPAddress(), GetAvailableLocalPort().ToString(), guid);
            var sensor = new Sensor
            {
                IPAddress = $"{sensorEmulator.GetIp()}:{sensorEmulator.GetPort()}",
            };
            return (sensorEmulator, sensor);
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        public static int GetAvailableLocalPort()
        {

            for (var port = 10000; port <= 11000; port++)
            {
                bool isAvailable = true;
                if (Devices.Any(f => f.GetPort() == port.ToString()))
                {
                    isAvailable = false;
                }
                IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
                TcpConnectionInformation[] tcpConnInfoArray = ipGlobalProperties.GetActiveTcpConnections();

                foreach (TcpConnectionInformation tcpi in tcpConnInfoArray)
                {
                    if (tcpi.LocalEndPoint.Port == port)
                    {
                        isAvailable = false;
                        break;
                    }
                }
                if (isAvailable)
                {
                    return port;
                }
            }
            throw new Exception("Free port not found");
        }
    }
}