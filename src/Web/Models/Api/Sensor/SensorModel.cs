using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Enum;

namespace Web.Models.Api.Sensor
{
    public class SensorModel
    {
        public int Id { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public PollutionLevel? PollutionLevel { get; set; }

        public List<SensorReadingModel> Readings { get; set; } 
    }
}