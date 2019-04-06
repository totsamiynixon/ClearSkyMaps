using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Data.Models;
using Web.Enum;
using Web.Models.Cache;

namespace Web.Helpers
{
    public static class PollutionHelper
    {
        private static Random _random = new Random();

        public static PollutionLevel CalculatePollutionLevel(List<Reading> readings)
        {
            if (!readings.Any())
            {
                return PollutionLevel.Unknown;
            }
            return (PollutionLevel)_random.Next(0, 2);
        }
    }
}