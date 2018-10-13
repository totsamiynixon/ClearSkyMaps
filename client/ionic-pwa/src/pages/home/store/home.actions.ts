import { Action } from "@ngrx/store";
import { Parameters } from "../../../models/parameters.enum";

export const SET_PARAMETER: string = "SET_PARAMETER";

export class SetParameterAction implements Action {
  readonly type = SET_PARAMETER;
  constructor(public payload: Parameters) {}
}
