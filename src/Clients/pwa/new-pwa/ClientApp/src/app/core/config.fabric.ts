import { IConfig } from "./models/config.model";
import { InjectionToken } from "@angular/core";

declare const APP_WEBSERVER_URL: any;
declare const APP_MAP_API_KEY: any;
export default {
    applicationServerUrl: APP_WEBSERVER_URL,
    hubPath: APP_WEBSERVER_URL + "readingsHub",
    map: {
        options: {
            center: {
                lat: 53.904502,
                lng: 27.561261
            },
            disableDefaultUI: true,
            zoom: 12,
            scrollwheel: false,
            mapTypeControl: false
        },
        key: APP_MAP_API_KEY
    }
}


export let APP_CONFIG = new InjectionToken<IConfig>('app.config');