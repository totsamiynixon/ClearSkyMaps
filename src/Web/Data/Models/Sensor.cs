﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Data.Models
{
    public class Sensor
    {
        public int Id { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public virtual List<Reading> Readings { get; set; }

        public string TrackingKey { get; set; }
    }
}