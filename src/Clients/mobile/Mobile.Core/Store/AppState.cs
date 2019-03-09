
using Mobile.Models;
using Mobile.Models.Enums;
using Redux;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mobile.Core.Store
{
    public class AppState : ICloneable

    {
        public Parameters FilterByParameter { get; set; }

        public IEnumerable<Sensor> Sensors { get; set; }

        public IAction LastAction { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public Sensor GetSensorById(int sensorId)
        {
            return Sensors?.FirstOrDefault(s => s.Id == sensorId);
        }
    }
}
