import { NgModule } from "@angular/core";
import { IonicPageModule } from "ionic-angular";
import { HomePage } from "./containers/home/home";
import { TranslateModule } from "@ngx-translate/core";
import { StoreModule } from "@ngrx/store";
import { Config } from "../../models/providers/config";
import { DetailsModalModule } from "./containers/details/details.module";
import { homeReducer } from "./store/home.reducer";
import {
  DefaultApiProvider,
  SignalRHubProvider,
  GoogleMapBuilder
} from "../../providers/implementations";
import {
  HubProvider,
  ApiProvider,
  MapBuilder
} from "../../providers/inerfaces";
import { Geolocation } from "@ionic-native/geolocation";
import  {GoogleMaps} from "@ionic-native/google-maps";
@NgModule({
  declarations: [HomePage],
  imports: [
    DetailsModalModule,
    IonicPageModule.forChild(HomePage),
    TranslateModule.forChild(),
    StoreModule.forRoot({ homeState: homeReducer })
  ],
  entryComponents: [HomePage],
  providers: [
    Geolocation,
    GoogleMaps,
    { provide: ApiProvider, useClass: DefaultApiProvider },
    { provide: MapBuilder, useClass: GoogleMapBuilder },
    { provide: HubProvider, useClass: SignalRHubProvider }
  ]
})
export class HomePageModule {}
