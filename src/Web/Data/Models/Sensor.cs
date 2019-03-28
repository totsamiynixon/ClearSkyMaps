using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Enum;

namespace Web.Data.Models
{
    public class Sensor
    {
        public int Id { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public virtual List<Reading> Readings { get; set; }

        public string TrackingKey { get; set; }

        public bool IsActive { get; set; }

        public bool IsVisible { get; set; }

        public bool IsDeleted { get; set; }

        public SensorType Type { get; set; }

        public string IPAddress { get; set; }
    }
}