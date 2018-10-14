import { Injectable } from "@angular/core";
import { HubConnectionBuilder, LogLevel, HubConnection } from "@aspnet/signalr";
import { Observable } from "rxjs/Observable";
import { Config } from "../../../models/providers/config";
import { HubProvider } from "../../inerfaces";

@Injectable()
export class SignalRHubProvider implements HubProvider {
  constructor(public config: Config) {}

  public getHub(): Promise<HubConnection> {
    return new Promise((resolve, reject) => {
      try {
        let hub = new HubConnectionBuilder()
          .withUrl(this.config.hubPath)
          .configureLogging(LogLevel.Information)
          .build();
        resolve(hub);
      } catch (ex) {
        reject(ex);
      }
    });
  }
}
