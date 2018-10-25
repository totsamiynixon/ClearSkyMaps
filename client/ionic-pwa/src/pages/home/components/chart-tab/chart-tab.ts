import { Component, ViewChild, ElementRef } from "@angular/core";
import { ChartBuilder } from "../../../../providers/inerfaces";
import { NavParams } from "ionic-angular";
import { TabModel } from "../../models/tab.model";

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
  chartModel: TabModel;
  @ViewChild("chart")
  chartRef: ElementRef;
  constructor(
    private chartBuilder: ChartBuilder,
    private navParams: NavParams
  ) {
    this.chartModel = this.navParams.get("model");
  }

  ionViewDidLoad() {
    let model = this.chartBuilder.initChart(this.chartRef.nativeElement);
    this.chartBuilder.updateDataset(
      this.chartModel.sensor,
      this.chartModel.filterByParameter
    );
  }
}
