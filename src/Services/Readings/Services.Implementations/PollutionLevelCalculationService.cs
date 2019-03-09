using Readings.Services.DTO.Enums;
using Readings.Services.DTO.PollutionCalculator;
using Readings.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readings.Services.Implementations
{
    public class PollutionLevelCalculationService : IPollutionLevelCalculationService
    {
        private static Random _random = new Random();
        public  PollutionLevel Calculate(IEnumerable<ReadingForPollutionCalculation> readings)
        {
            return (PollutionLevel)_random.Next(0, 3);
        }
    }
}
