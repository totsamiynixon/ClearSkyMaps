﻿
using Readings.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readings.Domain
{
    public class Reading : IEntity
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
        public Sensor Sensor { get; set; }
        public int SensorId { get; set; }
    }
}
