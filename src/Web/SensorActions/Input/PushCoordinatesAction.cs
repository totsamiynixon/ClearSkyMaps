﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Enum;

namespace Web.SensorActions.Input
{
    public class PushCoordinatesAction : InputSensorAction<PushCoordinatesActionPayload>
    {
        public PushCoordinatesAction(PushCoordinatesActionPayload payload) : base(InputSensorActionType.PushCoordinates, payload)
        {
        }
    }
}