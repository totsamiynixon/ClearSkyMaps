import { TableTab } from "./table-tab/table-tab";
import { ChartTab } from "./chart-tab/chart-tab";
import { Component } from "@angular/core";
import {
  IonicPage,
  NavController,
  NavParams,
  ModalController
} from "ionic-angular";

/**
 * Generated class for the DetailsPage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */

@IonicPage()
@Component({
  selector: "details-modal",
  templateUrl: "details.html"
})
export class DetailsModal {
  tableTab: any;
  chartTab: any;

  constructor(
    public navCtrl: NavController,
    public navParams: NavParams,
    public modalCtrl: ModalController
  ) {
    this.tableTab = TableTab;
    this.chartTab = ChartTab;
  }

  ionViewDidLoad() {
    console.log("ionViewDidLoad DetailsPage");
  }
  dismissModal() {
    this.navCtrl.pop();
  }
}
