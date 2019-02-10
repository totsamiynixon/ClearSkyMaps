import { createSelector, createFeatureSelector } from "@ngrx/store";
import { IHomePageState } from "./home.model";

export const getHomePageState = createFeatureSelector<IHomePageState>("homePage");

export const getSensorById = (sensorId: number) =>
  createSelector(getHomePageState, state => {
    return state.sensors.find(f => f.id == sensorId)
  });