import { HubConnection } from "@aspnet/signalr";
import { Injectable } from "@angular/core";
@Injectable()
export abstract class HubProvider {
  abstract getHub(): Promise<HubConnection>;
}
