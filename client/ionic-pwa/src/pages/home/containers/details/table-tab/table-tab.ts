

import { Component } from "@angular/core";
import { Reading } from "../../../../../models/reading.model";

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

  constructor() {
    let reading = new Reading();
    reading.created = new Date();
    this.readings = new Array<Reading>();
    this.readings.push(reading);
  }
}
