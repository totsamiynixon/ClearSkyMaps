﻿using ClearSkyMaps.Mobile.Models;
using Redux;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClearSkyMaps.CP.Mobile.Store.Actions
{
    public class SetSensorsAction : IAction
    {
        public IEnumerable<Sensor> Payload { get; private set; }

        public SetSensorsAction(IEnumerable<Sensor> payload)
        {
            Payload = payload;
        }
    }
}
