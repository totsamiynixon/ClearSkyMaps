import { platformBrowserDynamic } from "@angular/platform-browser-dynamic";
import { AppModule } from "./app.module";
import { ConfigProvider } from "../providers/implementations";
platformBrowserDynamic().bootstrapModule(AppModule);
