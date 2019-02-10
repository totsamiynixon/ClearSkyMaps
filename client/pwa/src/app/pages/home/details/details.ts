import { Observable } from "rxjs/internal/Observable";
import { Sensor } from "src/app/core/models/sensor.model";
import { Component, ChangeDetectorRef } from "@angular/core";
import { NavController, NavParams, ModalController } from "@ionic/angular";
import { Store } from "@ngrx/store";
import { ChartService } from "src/app/core/services/chart.service";
// import { TableTab } from "./table-tab/table-tab";
// import { ChartTab } from "./chart-tab/chart-tab";
import { ofType, Actions } from "@ngrx/effects";
import { HomePageActionTypes, UpdateSensor } from "src/app/state/home/home.actions";
import { tap, first } from "rxjs/operators";
import { getSensorById } from "src/app/state/home";
import { IAppState } from "src/app/state/app.reducer";


/**
 * Generated class for the DetailsPage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */

@Component({
  selector: "details-modal",
  templateUrl: "details.html"
})
export class DetailsModal {
  tableTab: any;
  chartTab: any;
  tabParams: Object;
  sensor: Observable<Sensor>
  constructor(
    public navCtrl: NavController,
    public navParams: NavParams,
    public modalCtrl: ModalController,
    private actions: Actions,
    private store: Store<IAppState>,
    private cd: ChangeDetectorRef,
    private chartService: ChartService
  ) {
    // this.tableTab = TableTab;
    // this.chartTab = ChartTab;
    this.tabParams = { model: null };
    //
    let id = navParams.get("sensorId") as number | null;
    if (id != null) {
      this.actions
        .pipe(
          ofType(HomePageActionTypes.UPDATE_SENSOR),
          tap((action: UpdateSensor) => {
            if (action.sensorId == id) {
              this.store.select(getSensorById(action.sensorId)).pipe(
                first(),
              ).subscribe(s => this.chartService.updateDataset(s));
            }
          })
        );
    }
  }
  dismissModal() {
    this.navCtrl.pop();
  }

  tabChanged() {
    this.cd.detectChanges();
  }
}
