using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Enum;

namespace Web.Areas.Admin.Emulation
{
    public class SetSensorStateModel
    {
        public string TrackingKey { get; set; }

        public bool IsActive { get; set; }

        public SensorType Type { get; set; }

        public string WebServerUrl { get; set; }
    }
}