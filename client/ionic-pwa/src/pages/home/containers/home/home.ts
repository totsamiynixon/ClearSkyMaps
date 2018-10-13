import { Parameters } from "./../../models/parameters.enum";
import { ApiProvider } from "./../../providers/api/apiProvider";
import { Observable } from "rxjs/Observable";
import { IHomePageState } from "./../../stores/home.module.store/interfaces/home.state";
import { HubDispatchModel } from "./../../models/providers/dispatch-reading.model";
import { HubProvider } from "./../../providers/hub/hubProvider";
import { MapItemModel } from "./../../models/pages/home/map-item.model";
import { MapBuilder } from "./../../providers/map/mapBuilder";
import { DetailsModal } from "./details/details";

import { Component, ViewChild, ElementRef } from "@angular/core";
import {
  IonicPage,
  NavController,
  NavParams,
  ModalController,
  ToastController,
  ActionSheetController
} from "ionic-angular";
import { Store, select } from "@ngrx/store";
import { Sensor } from "../../models/sensor.model";
import { SetParameterAction } from "../../stores/home.module.store/implementations/home.actions";

/**
 * Generated class for the HomePage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */

@IonicPage()
@Component({
  selector: "page-home",
  templateUrl: "home.html"
})
export class HomePage {
  @ViewChild("map")
  mapRef: ElementRef;
  mapItems: Array<MapItemModel> = [];
  uiComponentsVisible: boolean = true;
  constructor(
    private navCtrl: NavController,
    private navParams: NavParams,
    private modalCtrl: ModalController,
    private mapBuilder: MapBuilder,
    private hubProvider: HubProvider,
    private store: Store<IHomePageState>,
    private toastCtrl: ToastController,
    private apiProvider: ApiProvider,
    private actionSheetCtrl: ActionSheetController
  ) {}

  ionViewDidLoad() {
    console.log("322");
    this.mapBuilder.initMap(this.mapRef.nativeElement).then(
      () => {
        this.mapBuilder.drag().subscribe((eventType: string) => {
          switch (eventType) {
            case "drag":
              this.hideUiComponents();
            case "dragend":
              this.showUIComponents();
          }
        });
        let hub = this.hubProvider.getHub();
        hub.on("DispatchReadingAsync", (hubDispatchModel: HubDispatchModel) => {
          let mapItem = this.mapItems.find(item => {
            if (item.sensor.id == hubDispatchModel.sensorId) {
              return true;
            }
          });
          if (mapItem == null) {
            return;
          }
          mapItem.sensor.latestPollutionLevel =
            hubDispatchModel.latestPollutionLevel;
          mapItem.sensor.readings.unshift(hubDispatchModel.reading);
          if (mapItem.sensor.readings.length > 10) {
            mapItem.sensor.readings.pop();
          }
          this.mapBuilder.updateArea(
            mapItem.areaId,
            mapItem.sensor.latestPollutionLevel,
            mapItem.sensor.readings[0][this.getState().parameter]
          );
        });
        hub.start().then(() => {
          this.apiProvider.getAllSensors().subscribe(response => {
            this.mapItems = response.map(sensor => {
              return {
                sensor,
                areaId: this.mapBuilder.createNewArea(
                  sensor.latestPollutionLevel,
                  sensor.latitude,
                  sensor.longitude,
                  sensor.readings[0][this.getState().parameter],
                  () => {
                    this.openDetailsSheet(sensor);
                  }
                )
              };
            });
          });
        });
      },
      (error: Error) => {
        this.toastCtrl.create({
          message: error.message,
          duration: 3000,
          position: "top"
        });
      }
    );
  }

  openDetailsSheet(sensor: Sensor) {
    let currentParameter = this.getState().parameter;
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
            this.store.dispatch(new SetParameterAction(Parameters.cO2));
          }
        },
        {
          text: "LPG",
          cssClass: setClass(Parameters.lpg),
          handler: () => {
            this.store.dispatch(new SetParameterAction(Parameters.lpg));
          }
        },
        {
          text: "CO",
          cssClass: setClass(Parameters.co),
          handler: () => {
            this.store.dispatch(new SetParameterAction(Parameters.co));
          }
        },
        {
          text: "Пыль",
          cssClass: setClass(Parameters.dust),
          handler: () => {
            this.store.dispatch(new SetParameterAction(Parameters.dust));
          }
        }
      ]
    });
    actionSheet.present();
  }
  showModal() {
    const modal = this.modalCtrl.create(DetailsModal);
    modal.present();
  }

  hideUiComponents() {
    this.uiComponentsVisible = false;
  }

  showUIComponents() {
    this.uiComponentsVisible = true;
  }
  private getState(): IHomePageState {
    let state: IHomePageState;

    this.store.take(1).subscribe(s => (state = s));

    return state;
  }
}
