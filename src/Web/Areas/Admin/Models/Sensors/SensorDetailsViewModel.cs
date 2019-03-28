using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Enum;

namespace Web.Areas.Admin.Models.Sensors
{
    public class SensorDetailsViewModel
    {
        public string IPAddress { get; set; }

        public string TrackingKey { get; set; }

        public bool IsActive { get; set; }

        public bool IsVisible { get; set; }

        public SensorType Type { get; set; }
    }
}