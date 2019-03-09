import { TabModel } from "./../../models/tab.model";
import { getFilterByParameter } from "./../../store/home.reducer";
import { Parameters } from "./../../../../models/parameters.enum";
import { Component, ChangeDetectorRef } from "@angular/core";
import { Reading } from "../../../../models/reading.model";
import { NavParams } from "ionic-angular";
import { Sensor } from "../../../../models/sensor.model";

/**
 * Generated class for the TableTabComponent component.
 *
 * See https://angular.io/api/core/Component for more info on Angular
 * Components.
 */
@Component({
  selector: "table-tab",
  templateUrl: "table-tab.html"
})
export class TableTab {
  expanded: boolean = false;
  tabModel: TabModel;
  constructor(private navParams: NavParams, private cd: ChangeDetectorRef) {
    this.tabModel = this.navParams.get("model") as TabModel;
  }

  changeExpand() {
    this.expanded = !this.expanded;
    this.cd.detectChanges();
  }
  checkHighlight(param: string): boolean {
    return (
      param.toLowerCase() ==
      Parameters[this.tabModel.filterByParameter].toLowerCase()
    );
  }
}
