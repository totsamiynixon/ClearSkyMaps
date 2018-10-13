import { ConfigProvider } from "./../../providers/config/configProvider";
import { ChartBuilder } from "./../../providers/chart/chartBuilder";
import { MapBuilder } from "./../../providers/map/mapBuilder";

import { NgModule } from "@angular/core";
import { IonicPageModule } from "ionic-angular";
import { HomePage } from "./containers/home/home";
import { TranslateModule } from "@ngx-translate/core";
import { StoreModule } from "@ngrx/store";
import { homeReducer } from "../../stores/home.module.store/implementations/home.reducer";
import { ApiProvider } from "../../providers/api/apiProvider";
import { HubProvider } from "../../providers/hub/hubProvider";
import { Config } from "../../models/providers/config";
import { DetailsModalModule } from "./containers/details/details.module";

@NgModule({
  declarations: [HomePage],
  imports: [
    DetailsModalModule,
    IonicPageModule.forChild(HomePage),
    TranslateModule.forChild(),
    StoreModule.forRoot({ homeState: homeReducer })
  ],
  providers: [ApiProvider, HubProvider, MapBuilder, ChartBuilder]
})
export class HomePageModule {}
