﻿using Mobile.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mobile.Models.Hub
{
    public class HubDispatchModel
    {
        public int SensorId { get; set; }

        public PollutionLevels LatestPollutionLevel { get; set; }

        public Reading Reading { get; set; }
    }
}
