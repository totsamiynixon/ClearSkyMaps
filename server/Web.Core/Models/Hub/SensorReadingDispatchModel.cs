using Services.DTO.Enums;
using Services.DTO.Models.Reading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Core.Models.Hub
{
    public class SensorReadingDispatchModel
    {
        public int SensorId { get; set; }

        public PollutionLevel LatestPollutionLevel { get; set; }

        public SensorReadingDTO Reading { get; set; }
    }
}