using Services.DTO.Attributes;
using Services.DTO.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO.PollutionCalculator
{
    public class ReadingForPollutionCalculation
    {
        public int Id { get; set; }
        [PDK(300, LevelOfDanger.UltraHigh)]
        public float CO2 { get; set; }
        [PDK(300, LevelOfDanger.UltraHigh)]
        public float LPG { get; set; }
        [PDK(3.0, LevelOfDanger.High)]
        public float CO { get; set; }
        [PDK(0.716, LevelOfDanger.Medium)]
        public float CH4 { get; set; }
        public float Dust { get; set; }
        public float Temp { get; set; }
        public float Hum { get; set; }
        public float Preassure { get; set; }
    }
}
