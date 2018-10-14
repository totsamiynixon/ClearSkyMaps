import { TabModel } from "./../../models/tab.model";
import { Component, ChangeDetectorRef } from "@angular/core";
import {
  IonicPage,
  NavController,
  NavParams,
  ModalController,
  ActionSheetController
} from "ionic-angular";
import { TableTab } from "../../components/table-tab/table-tab";
import { ChartTab } from "../../components/chart-tab/chart-tab";
import { Sensor } from "../../../../models/sensor.model";
import { Observable } from "rxjs/Observable";
import { Store, select } from "@ngrx/store";
import { IHomePageState } from "../../store/home.state";
import { getSensorById } from "../../store/home.reducer";
import { Parameters } from "../../../../models/parameters.enum";
import { SetFilterParameterAction } from "../../store/home.actions";

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
  tabParams: TabModel;
  filterByParameter: string;
  constructor(
    public navCtrl: NavController,
    public navParams: NavParams,
    public modalCtrl: ModalController,
    private store: Store<IHomePageState>,
    private actionSheetCtrl: ActionSheetController,
    private cd: ChangeDetectorRef
  ) {
    this.tableTab = TableTab;
    this.chartTab = ChartTab;
    this.tabParams = new TabModel();
    let id = navParams.get("sensorId") as number | null;
    if (id != null) {
      this.store
        .pipe(select("homeState"))
        .subscribe((state: IHomePageState) => {
          this.tabParams.sensor = state.sensors.find(f => f.id == id);
          this.tabParams.filterByParameter = state.filterByParameter;
          this.filterByParameter = Parameters[state.filterByParameter];
        });
    }
  }

  ionViewDidLoad() {
    console.log("ionViewDidLoad DetailsPage");
  }
  dismissModal() {
    this.navCtrl.pop();
  }
  showParamsSheet() {
    let currentParameter = this.tabParams.filterByParameter;
    const setClass = (parameter: Parameters) => {
      if (parameter == currentParameter) {
        return "active";
      }
      return "";
    };
    const actionSheet = this.actionSheetCtrl.create({
      title: "Выберите параметр",
      buttons: [
        {
          text: "СO2",
          cssClass: setClass(Parameters.cO2),
          handler: () => {
            this.store.dispatch(new SetFilterParameterAction(Parameters.cO2));
          }
        },
        {
          text: "LPG",
          cssClass: setClass(Parameters.lpg),
          handler: () => {
            this.store.dispatch(new SetFilterParameterAction(Parameters.lpg));
          }
        },
        {
          text: "CO",
          cssClass: setClass(Parameters.co),
          handler: () => {
            this.store.dispatch(new SetFilterParameterAction(Parameters.co));
          }
        },
        {
          text: "Пыль",
          cssClass: setClass(Parameters.dust),
          handler: () => {
            this.store.dispatch(new SetFilterParameterAction(Parameters.dust));
          }
        }
      ]
    });
    actionSheet.present();
  }

  tabChanged() {
    this.cd.detectChanges();
  }
}
