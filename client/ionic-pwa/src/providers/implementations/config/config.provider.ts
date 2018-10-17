import { Config, ConfigMap } from "../../../models/providers/config";
declare const APP_WEBSERVER_URL: any;
declare const APP_MAP_API_KEY: any;
export class ConfigProvider {
  static getConfig(): Config {
    let config = new Config();
    config.applicationServerUrl = APP_WEBSERVER_URL;
    config.hubPath = config.applicationServerUrl + "readingsHub";
    config.map = new ConfigMap();
    config.map.options = config.map.options = {
      center: {
        lat: 53.904502,
        lng: 27.561261
      },
      disableDefaultUI: true,
      zoom: 12,
      scrollwheel: false,
      mapTypeControl: false
    };
    config.map.key = APP_MAP_API_KEY;
    return config;
  }
}
