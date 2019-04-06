﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Enum;

namespace Web.SensorActions.Output
{
    public class PullCoordinatesAction : OutputSensorAction<PullCoordinatesActionPayload>
    {
        public PullCoordinatesAction(PullCoordinatesActionPayload payload) : base(OuputSensorActionType.PullCoordinates, payload)
        {
        }
    }
}