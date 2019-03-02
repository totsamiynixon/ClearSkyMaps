import { Injectable, Inject } from "@angular/core";
import { HubConnectionBuilder, LogLevel, HubConnection } from "@aspnet/signalr";
import { IConfig } from "../models/config.model";
import { APP_CONFIG } from "../config.fabric";

@Injectable({
  providedIn: "root"
})
export class SignalRHubService {
  constructor(@Inject(APP_CONFIG) private config: IConfig) {}

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
