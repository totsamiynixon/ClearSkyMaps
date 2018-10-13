import { Injectable } from "@angular/core";
import { HubConnectionBuilder, LogLevel, HubConnection } from "@aspnet/signalr";
import { Observable } from "rxjs/Observable";
import { Config } from "../../../models/providers/config";

@Injectable()
export class HubProvider {
  hubConnection: HubConnection;
  constructor(public config: Config) {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.config.hubPath)
      .configureLogging(LogLevel.Information)
      .build();
  }

  public getHub(): HubConnection {
    return this.hubConnection;
  }
}
