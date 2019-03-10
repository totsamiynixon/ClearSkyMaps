using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Data.Models;
using Web.Enum;

namespace Web.Models.Cache
{
    public class SensorCacheItemModel
    {

        public SensorCacheItemModel(Sensor sensor, PollutionLevel? pollutionLevel)
        {
            Sensor = sensor;
            PollutionLevel = pollutionLevel;
        }

        public Sensor Sensor { get; set; }

        public PollutionLevel? PollutionLevel { get; set; }
    }
}