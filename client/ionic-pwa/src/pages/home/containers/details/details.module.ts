import { DetailsModal } from "./details";
import { NgModule } from "@angular/core";
import { IonicPageModule } from "ionic-angular";
import { TranslateModule } from "@ngx-translate/core";
import { TableTab } from "../../components/table-tab/table-tab";
import { ChartTab } from "../../components/chart-tab/chart-tab";
import { ChartJSChartBuilder } from "../../../../providers/implementations";
import { ChartBuilder } from "../../../../providers/inerfaces";
import { StoreModule } from "@ngrx/store";
import { homeReducer } from "../../store/home.reducer";

@NgModule({
  declarations: [DetailsModal, TableTab, ChartTab],
  entryComponents: [TableTab, ChartTab],
  imports: [
    IonicPageModule.forChild(DetailsModal),
    TranslateModule.forChild(),
    StoreModule.forRoot({ homeState: homeReducer })
  ],
  providers: [{ provide: ChartBuilder, useClass: ChartJSChartBuilder }]
})
export class DetailsModalModule {}
