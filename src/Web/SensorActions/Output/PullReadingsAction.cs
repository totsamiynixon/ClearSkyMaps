﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Enum;

namespace Web.SensorActions.Output
{
    public class PullReadingsAction : OutputSensorAction<PullReadingsActionPayload>
    {
        public PullReadingsAction(PullReadingsActionPayload payload) : base(OuputSensorActionType.PullReadings, payload)
        {
        }
    }
}