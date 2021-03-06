﻿using Mobile.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mobile.Models
{
    public class Sensor
    {
        public int Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public List<Reading> Readings { get; set; }
        public PollutionLevels LatestPollutionLevel { get; set; }
    }
}
