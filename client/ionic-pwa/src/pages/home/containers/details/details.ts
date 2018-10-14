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
import { ChartBuilder } from "../../../../providers/inerfaces";

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
  tabParams: Object;
  filterByParameter: string;
  constructor(
    public navCtrl: NavController,
    public navParams: NavParams,
    public modalCtrl: ModalController,
    private store: Store<IHomePageState>,
    private actionSheetCtrl: ActionSheetController,
    private cd: ChangeDetectorRef,
    private chartBuilder: ChartBuilder
  ) {
    this.tableTab = TableTab;
    this.chartTab = ChartTab;
    this.tabParams = { model: null };
    let id = navParams.get("sensorId") as number | null;
    if (id != null) {
      this.store
        .pipe(select("homeState"))
        .subscribe((state: IHomePageState) => {
          console.log("Details catched state chage", state);
          if (this.tabParams["model"] == null) {
            this.tabParams["model"] = new TabModel();
          }
          let model = this.tabParams["model"] as TabModel;
          model.sensor = state.sensors.find(f => f.id == id);
          model.filterByParameter = state.filterByParameter;
          this.tabParams["model"] = model;
          this.filterByParameter = Parameters[state.filterByParameter];
          try {
            this.chartBuilder.updateDataset(
              model.sensor,
              model.filterByParameter
            );
          } catch (ex) {}
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
