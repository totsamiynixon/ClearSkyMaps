import signalR from "@aspnet/signalr";
import config from "app-config";
const hub = signalR
  .HubConnectionBuilder()
  .withUrl(config.baseUrl + config.hubName)
  .configureLogging(signalR.LogLevel.Information)
  .build();

export default hub;
