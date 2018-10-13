import { HubConnection } from "@aspnet/signalr";

export interface IHubProvider {
  getHub(): HubConnection;
}
