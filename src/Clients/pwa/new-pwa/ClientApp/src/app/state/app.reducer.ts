import { ActionReducerMap, MetaReducer, Action } from "@ngrx/store";
import * as fromHome from "./home/home.reducer";
import { IHomePageState } from "./home/home.model";

export interface IAppState {
  homePage: IHomePageState;
}
export const appReducer: ActionReducerMap<IAppState> = {
  homePage: fromHome.reducer
};
