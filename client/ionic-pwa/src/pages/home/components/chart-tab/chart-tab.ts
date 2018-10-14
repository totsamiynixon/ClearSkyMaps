import { Component } from "@angular/core";

/**
 * Generated class for the ChartTabComponent component.
 *
 * See https://angular.io/api/core/Component for more info on Angular
 * Components.
 */
@Component({
  selector: "chart-tab",
  templateUrl: "chart-tab.html"
})
export class ChartTab {
  text: string;

  constructor() {
    console.log("Hello ChartTabComponent Component");
    this.text = "Вкладка графика";
  }
}
