﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Data.Models;
using Web.Enum;

namespace Web.Models.Hub
{
    public class SensorReadingDispatchModel
    {
        public int SensorId { get; set; }

        public ReadingDispatchModel Reading { get; set; }

        public PollutionLevel? PollutionLevel { get; set; }
    }

    public class ReadingDispatchModel
    {
        public int Id { get; set; }
        public float CO2 { get; set; }
        public float LPG { get; set; }
        public float CO { get; set; }
        public float CH4 { get; set; }
        public float Dust { get; set; }
        public float Temp { get; set; }
        public float Hum { get; set; }
        public float Preassure { get; set; }
        public DateTime Created { get; set; }
    }
}