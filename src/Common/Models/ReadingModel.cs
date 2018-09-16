using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class ReadingModel
    {
        public float CO2 { get; set; }
        public float LPG { get; set; }
        public float CO { get; set; }
        public float CH4 { get; set; }
        public float Temp { get; set; }
        public float Hum { get; set; }
        public float Dust { get; set; }
        public float Preassure { get; set; }
        public string SensorTrackingKey { get; set; }
    }
}
