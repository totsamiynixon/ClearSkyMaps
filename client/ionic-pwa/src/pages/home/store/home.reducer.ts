import { Action } from "@ngrx/store";
import { SET_PARAMETER, SetParameterAction } from "./home.actions";
import { Parameters } from "../../../models/parameters.enum";
import { IHomePageState } from "../interfaces/home.state";
const currentState: IHomePageState = {
  parameter: Parameters.cO2
};

export function homeReducer(
  state: IHomePageState = currentState,
  action: Action
): IHomePageState {
  switch (action.type) {
    case SET_PARAMETER:
      return {
        ...state,
        parameter: (action as SetParameterAction).payload
      };
    default:
      return state;
  }
}
