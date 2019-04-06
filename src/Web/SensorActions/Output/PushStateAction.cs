﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Enum;

namespace Web.SensorActions.Output
{
    public class PushStateAction : OutputSensorAction<PushStateActionPayload>
    {
        public PushStateAction(PushStateActionPayload payload) : base(OuputSensorActionType.PushState, payload)
        {
        }
    }
}