import { Action, createSelector, createFeatureSelector } from "@ngrx/store";
import {
  SET_FILTER_PARAMETER,
  SetFilterParameterAction,
  SET_SENSORS,
  UPDATE_SENSOR,
  SetSensorsAction,
  UpdateSensorAction
} from "./home.actions";
import { Parameters } from "../../../models/parameters.enum";
import { IHomePageState } from "./home.state";
const currentState: IHomePageState = {
  filterByParameter: Parameters.cO2,
  sensors: [],
  lastAction: { type: null }
};

export function homeReducer(
  state: IHomePageState = currentState,
  action: Action
): IHomePageState {
  switch (action.type) {
    case SET_FILTER_PARAMETER:
      return {
        ...state,
        lastAction: action,
        filterByParameter: (action as SetFilterParameterAction).payload
      };
    case SET_SENSORS:
      return {
        ...state,
        lastAction: action,
        sensors: (action as SetSensorsAction).payload
      };
    case UPDATE_SENSOR: {
      let updateAction = action as UpdateSensorAction;
      let updateSensor = state.sensors.find(f => f.id == updateAction.sensorId);
      updateSensor.latestPollutionLevel = updateAction.pollutionLevel;
      updateSensor.readings.unshift(updateAction.payload);
      if (updateSensor.readings.length > 10) {
        updateSensor.readings.pop();
      }
      return {
        ...state,
        lastAction: action
      };
    }
    default:
      return state;
  }
}

export const getHomePageState = createFeatureSelector<IHomePageState>(
  "homeState"
);
export const getFilterByParameter = createSelector(
  getHomePageState,
  s => s.filterByParameter
);

export const getSensorById = (sensorId: number) =>
  createSelector(getHomePageState, state => {
    return {
      sensor: state.sensors.find(f => f.id == sensorId),
      filterByParameter: state.filterByParameter
    };
  });
