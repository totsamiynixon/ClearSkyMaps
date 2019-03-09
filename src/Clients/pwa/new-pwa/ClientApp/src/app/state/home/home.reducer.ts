import { IHomePageState } from './home.model';
import { HomePageActions, HomePageActionTypes, SetSensors, UpdateSensor, SetSensorsSuccess } from './home.actions';

export const initialState: IHomePageState = {
    sensors: []
};

export function reducer(
    state = initialState,
    action: HomePageActions
): IHomePageState {
    switch (action.type) {
        case HomePageActionTypes.SET_SENSORS_SUCCESS: {
            return {
                ...state,
                sensors: (action as SetSensorsSuccess).payload
            };
        }
        case HomePageActionTypes.UPDATE_SENSOR: {
            let updateAction = action as UpdateSensor;
            let updateSensor = state.sensors.find(f => f.id == updateAction.sensorId);
            updateSensor.latestPollutionLevel = updateAction.pollutionLevel;
            updateSensor.readings.unshift(updateAction.payload);
            if (updateSensor.readings.length > 10) {
                updateSensor.readings.pop();
            }
            return {
                ...state,
            };
        }
        default: {
            return state;
        }
    }
}
