import { HubConnectionBuilder, LogLevel } from "@aspnet/signalr";
import config from "@/config";
const hub = new HubConnectionBuilder()
  .withUrl(config.baseUrl + config.hubName)
  .configureLogging(LogLevel.Information)
  .build();

export default hub;
