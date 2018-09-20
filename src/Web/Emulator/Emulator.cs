using ArduinoServer.Controllers.Api;
using DAL.Intarfaces;
using Domain;
using Ninject.Planning.Bindings;
using Services.DTO.Reading;
using Services.DTO.Sensor;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Web.Emulator.Database;
using Web.Emulator.DI;
using Web.Infrastructure;

namespace Web.Emulator
{
    public static class Emulator
    {
        private static List<string> _trackingKeys;
        public static bool IsEmulationEnabled = false;
        private static Random _emulatorRandom = new Random();
        private static IEnumerable<IBinding> _bindings;
        private static EmulatorDependencyResolver _resolver;
        private static double memoryLimit = 5e+8;

        static Emulator()
        {
            Effort.Provider.EffortProviderConfiguration.RegisterProvider();
        }



        public static void RunEmulation()
        {
            if (!IsEmulationEnabled)
            {
                IsEmulationEnabled = true;
                SetBinding();
                SeedSensors();
                StartTask();
            }
        }

        public static void SetResolver(EmulatorDependencyResolver resolver)
        {
            _resolver = resolver;
        }

        public static void SetMemoryLimit(double limit)
        {
            memoryLimit = limit;
        }
        public static void StopEmulation()
        {
            if (IsEmulationEnabled)
            {
                IsEmulationEnabled = false;
                ClearSensors();
                ReleaseBindings();
            }
        }

        private static void StartTask()
        {
            var task = Task.Run(async () =>
            {
                while (IsEmulationEnabled)
                {
                    DispatchFakeData();
                    await Task.Delay(10000);
                    try
                    {
                        CheckMemory();
                    }
                    catch (OutOfMemoryException ex)
                    {
                        StopEmulation();
                    }
                }
            });
        }

        private async static void SeedSensors()
        {
            var service = _resolver.GetService<ISensorService>();
            _trackingKeys = new List<string>();
            var iterations = _emulatorRandom.Next(1, 20);
            for (int i = 0; i < iterations; i++)
            {
                _trackingKeys.Add(await service.RegisterAndGetTrackingKeyAsync(GetFakeSensor()));
            }
        }

        private async static void ClearSensors()
        {
            var context = _resolver.GetService<IDataContext>();
            var repo = context.GetRepository<Sensor>();
            repo.ToList().ForEach(s => repo.Remove(s));
            await context.SaveChangesAsync();
            _trackingKeys = null;
        }

        private static void DispatchFakeData()
        {
            Parallel.ForEach(_trackingKeys, async (key) =>
            {
                var controller = new ReadingsController(DependencyResolver.Current.GetService<IReadingService>(), DependencyResolver.Current.GetService<ISensorService>());
                var fakeReading = GetFakeReading();
                await controller.PostReading(key, fakeReading);
            });
        }

        private static SaveReadingDTO GetFakeReading()
        {
            return new SaveReadingDTO
            {
                CO2 = (float)Math.Round((float)_emulatorRandom.NextDouble() * 350, 3),
                LPG = (float)Math.Round((float)_emulatorRandom.NextDouble() * 350, 3),
                CO = (float)Math.Round((float)_emulatorRandom.NextDouble() * 4, 3),
                CH4 = (float)Math.Round((float)_emulatorRandom.NextDouble() * 0.716, 3),
                Dust = (float)Math.Round((float)_emulatorRandom.NextDouble() * 350, 3),
                Temp = (float)Math.Round((float)_emulatorRandom.NextDouble() * 40, 3),
                Preassure = (float)Math.Round((float)_emulatorRandom.NextDouble() * 20, 3),
                Hum = (float)Math.Round((float)_emulatorRandom.NextDouble() * 40, 3),
                Created = DateTime.UtcNow - new TimeSpan(0, 0, 10)
            };
        }


        private static RegisterSensorDTO GetFakeSensor()
        {
            var fakeLatLong = GetLocation(27.560597, 53.904588, 8000);
            var sensor = new RegisterSensorDTO
            {
                Latitude = fakeLatLong.randomLatitude,
                Longitude = fakeLatLong.randomLongitude,
            };
            return sensor;
        }

        private static (double randomLongitude, double randomLatitude) GetLocation(double longitude, double latittude, int radius)
        {
            Random random = _emulatorRandom;

            // Convert radius from meters to degrees
            double radiusInDegrees = radius / 111000f;

            double u = random.NextDouble();
            double v = random.NextDouble();
            double w = radiusInDegrees * Math.Sqrt(u);
            double t = 2 * Math.PI * v;
            double x = w * Math.Cos(t);
            double y = w * Math.Sin(t);

            // Adjust the x-coordinate for the shrinking of the east-west distances
            double new_x = x / Math.Cos((Math.PI / 180) * latittude);

            double foundLongitude = new_x + longitude;
            double foundLatitude = y + latittude;
            return (foundLongitude, foundLatitude);
        }

        private static void SetBinding()
        {
            if (_resolver == null)
            {
                throw new InvalidOperationException("There is no resolver to work with!");
            }
            var instanceID = $"emulation_{Guid.NewGuid().ToString()}";
            _resolver.RebuildDependencies((kernel) =>
            {
                _bindings = kernel.GetBindings(typeof(IDataContext));
                foreach (var binding in _bindings)
                {
                    kernel.RemoveBinding(binding);
                }
                kernel.Bind<IDataContext>().ToMethod((context) =>
                {
                    return new EmulatorContext(Effort.DbConnectionFactory.CreatePersistent(instanceID));
                });
            });
        }

        private static void ReleaseBindings()
        {
            if (_resolver == null)
            {
                throw new InvalidOperationException("There is no resolver to work with!");
            }
            if (_bindings == null)
            {
                throw new InvalidOperationException("There are no any bindings!");
            }
            _resolver.RebuildDependencies((kernel) =>
            {
                var bindingsToRemove = kernel.GetBindings(typeof(IDataContext));
                foreach (var binding in bindingsToRemove)
                {
                    kernel.RemoveBinding(binding);
                }
                foreach (var binding in _bindings)
                {
                    kernel.AddBinding(binding);
                }
                _bindings = null;
            });
        }

        private static void CheckMemory()
        {
            Process currentProcess = System.Diagnostics.Process.GetCurrentProcess();
            long totalBytesOfMemoryUsed = currentProcess.WorkingSet64;
            if (totalBytesOfMemoryUsed > memoryLimit)
            {
                throw new OutOfMemoryException("Too much memory used!");
            }
        }
    }
}