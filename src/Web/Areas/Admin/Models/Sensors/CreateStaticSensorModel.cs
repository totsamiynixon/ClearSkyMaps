﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Enum;

namespace Web.Areas.Admin.Models.Sensors
{
    public class CreateStaticSensorModel
    {
        public string IPAddress { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }
}