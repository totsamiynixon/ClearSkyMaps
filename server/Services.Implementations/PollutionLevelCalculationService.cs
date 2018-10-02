using Services.DTO.Enums;
using Services.DTO.PollutionCalculator;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public class PollutionLevelCalculationService : IPollutionLevelCalculationService
    {
        private static Random _random = new Random();
        public  PollutionLevel Calculate(IEnumerable<ReadingForPollutionCalculation> readings)
        {
            return (PollutionLevel)_random.Next(0, 2);
        }
    }
}
