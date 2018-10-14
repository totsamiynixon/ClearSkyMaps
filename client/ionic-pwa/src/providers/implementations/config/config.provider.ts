import { Config, ConfigMap } from "../../../models/providers/config";
declare var process: { env: { [key: string]: string | undefined } };
export class ConfigProvider {
  static getConfig(): Config {
    let config = new Config();
    config.applicationServerUrl =
      process.env["APP_WEBSERVER_URL_" + process.env.APP_STAGE] ||
      "http://localhost:56875/";
    config.hubPath = config.applicationServerUrl + "readingsHub";
    config.map = new ConfigMap();
    config.map.options = config.map.options = {
      center: {
        lat: 53.904502,
        lng: 27.561261
      },
      disableDefaultUI: true,
      zoom: 11,
      scrollwheel: false,
      mapTypeControl: false
    };
    config.map.key = "AIzaSyAfj-ARjZc7VEGb0_grdk5VFu5wXphQyjo";
    return config;
  }
}
