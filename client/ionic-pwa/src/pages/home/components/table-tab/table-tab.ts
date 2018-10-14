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
  readings: Reading[];
  expanded: boolean = false;

  constructor(private navParams: NavParams, private cd: ChangeDetectorRef) {
    this.readings = (this.navParams.get("sensor") as Sensor).readings;
  }

  changeExpand() {
    this.expanded = !this.expanded;
    this.cd.detectChanges();
  }
}
