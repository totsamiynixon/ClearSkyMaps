
using Mobile.Core.Store.Actions;
using Redux;
using System.Linq;

namespace Mobile.Core.Store
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
                var updateSensor = newState.Sensors.FirstOrDefault(f => f.Id == updateAction.SensorId);
                if (updateSensor == null)
                {
                    //throw new ArgumentNullException($"No such sensor to update {nameof(updateSensor)}");
                    return newState;
                }
                updateSensor.LatestPollutionLevel = updateAction.PollutionLevel;
                updateSensor.Readings.Add(updateAction.Payload);
                updateSensor.Readings = updateSensor.Readings.OrderByDescending(f => f.Created).Take(10).ToList();
                return newState;
            }
            return newState;
        }
    }
}
