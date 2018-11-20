﻿using ClearSkyMaps.Xamarin.Forms.Enums;
using Redux;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClearSkyMaps.Xamarin.Forms.Store.Home.Actions
{
    public class SetFilterParameterAction : IAction
    {
        public Parameters Payload { get; private set; }

        public SetFilterParameterAction(Parameters payload)
        {
            Payload = payload;
        }
    }
}
