using Readings.Services.DTO.Enums;
using Readings.Services.DTO.Models.Reading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Readings.API.Models.Hub
{
    public class SensorReadingDispatchModel
    {
        public int SensorId { get; set; }

        public PollutionLevel LatestPollutionLevel { get; set; }

        public SensorReadingDTO Reading { get; set; }
    }
}