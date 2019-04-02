using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Enum;

namespace Web.Areas.Admin.Models.Sensors
{
    public class CreateSensorModel
    {
        public string IPAddress { get; set; }

        public SensorType Type { get; set; }
    }
}