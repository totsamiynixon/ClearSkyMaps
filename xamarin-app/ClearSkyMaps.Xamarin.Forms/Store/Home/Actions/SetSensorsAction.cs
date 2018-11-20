using ClearSkyMaps.Xamarin.Forms.Models;
using Redux;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClearSkyMaps.Xamarin.Forms.Store.Home.Actions
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
