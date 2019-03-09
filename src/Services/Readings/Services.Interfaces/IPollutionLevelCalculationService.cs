
using Readings.Services.DTO.Enums;
using Readings.Services.DTO.PollutionCalculator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readings.Services.Interfaces
{
    public interface IPollutionLevelCalculationService
    {
        PollutionLevel Calculate(IEnumerable<ReadingForPollutionCalculation> readings);
    }
}
