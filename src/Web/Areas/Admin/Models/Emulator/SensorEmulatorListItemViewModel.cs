using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Enum;

namespace Web.Areas.Admin.Models.Emulator
{
    public class SensorEmulatorListItemViewModel
    {
        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public SensorType Type { get; set; }

        public bool IsActive { get; set; }

        public string TrackingKey { get; set; }

        public string WebServerIP { get; set; }

        public string WebSocketPath { get; set; }

        public bool IsMated { get; set; }

        public bool IsOn { get; set; }
    }
}