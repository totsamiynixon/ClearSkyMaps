using ClearSkyMaps.Xamarin.Forms.Enums;
using ClearSkyMaps.Xamarin.Forms.Models;
using Redux;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClearSkyMaps.Xamarin.Forms.Store.Home.Actions
{
    public class UpdateSensorAction : IAction
    {
        public Reading Payload { get; private set; }
        public int SensorId { get; private set; }
        public PollutionLevels PollutionLevel { get; private set; }
        public UpdateSensorAction(Reading payload, int sensorId, PollutionLevels pollutionLevel)
        {
            Payload = payload;
            SensorId = sensorId;
            PollutionLevel = pollutionLevel;
        }
    }
}
