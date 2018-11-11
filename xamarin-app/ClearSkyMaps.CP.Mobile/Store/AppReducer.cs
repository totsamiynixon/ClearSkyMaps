
using ClearSkyMaps.CP.Mobile.Store.Actions;
using ClearSkyMaps.Mobile.Models;

using Redux;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClearSkyMaps.CP.Mobile.Store
{
    public static class AppReducer
    {
        public static AppState Execute(AppState state, IAction action)
        {
            var newState = (AppState)state.Clone();
            newState.LastAction = action;
            if (action is SetFilterParameterAction)
            {
                newState.FilterByParameter = (action as SetFilterParameterAction).Payload;
                return newState;
            }
            if (action is SetSensorsAction)
            {
                newState.Sensors = (action as SetSensorsAction).Payload;
                return newState;
            }
            if (action is UpdateSensorAction)
            {
                var updateAction = action as UpdateSensorAction;
                var updateSensor = state.Sensors.FirstOrDefault(f => f.Id == updateAction.SensorId);
                if (updateSensor == null)
                {
                    //throw new ArgumentNullException($"No such sensor to update {nameof(updateSensor)}");
                    return newState;
                }
                updateSensor.LatestPollutionLevel = updateAction.PollutionLevel;
                var newList = new List<Reading> {
                    updateAction.Payload
                };
                newList.AddRange(updateSensor.Readings);
                updateSensor.Readings = newList;
                if (updateSensor.Readings.Count > 10)
                {
                    newList.RemoveAt(newList.Count - 1);
                }
                return newState;
            }
            return newState;
        }
    }
}
