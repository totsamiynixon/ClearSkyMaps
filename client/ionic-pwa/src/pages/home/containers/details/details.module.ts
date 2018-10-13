import { DetailsModal } from "./details";
import { TableTab } from "./table-tab/table-tab";
import { NgModule } from "@angular/core";
import { IonicPageModule } from "ionic-angular";
import { TranslateModule } from "@ngx-translate/core";
import { ChartTab } from "./chart-tab/chart-tab";

@NgModule({
  declarations: [DetailsModal, TableTab, ChartTab],
  entryComponents: [TableTab, ChartTab],
  imports: [IonicPageModule.forChild(DetailsModal), TranslateModule.forChild()]
})
export class DetailsModalModule {}
