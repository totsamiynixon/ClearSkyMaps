using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Readings.API.Models.Api.Readings
{
    public class ApiPostReadingModel
    {
        [Required]
        public float CO2 { get; set; }
        [Required]
        public float LPG { get; set; }
        [Required]
        public float CO { get; set; }
        [Required]
        public float CH4 { get; set; }
        [Required]
        public float Dust { get; set; }
        [Required]
        public float Temp { get; set; }
        [Required]
        public float Hum { get; set; }
        [Required]
        public float Preassure { get; set; }
        [Required]
        public string SensorTrackingKey { get; set; }
    }
}
