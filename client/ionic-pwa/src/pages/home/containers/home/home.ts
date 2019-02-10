import { IHomePageState } from "./../../store/home.state";
import {
  SetFilterParameterAction,
  SetSensorsAction,
  UpdateSensorAction,
  SET_SENSORS
} from "./../../store/home.actions";

import {
  Component,
  ViewChild,
  ElementRef,
  ChangeDetectorRef
} from "@angular/core";
import {
  ModalController,
  ActionSheetController
} from "ionic-angular";
import { Store, select } from "@ngrx/store";
import {
  MapBuilder,
  ApiProvider,
  HubProvider,
  AlertsService
} from "../../../../providers/inerfaces";
import { HubDispatchModel } from "../../../../models/providers/dispatch-reading.model";
import { Parameters } from "../../../../models/parameters.enum";
import { DetailsModal } from "../details/details";
import { getFilterByParameter } from "../../store/home.reducer";

@Component({
  selector: "page-home",
  templateUrl: "home.html"
})
export class HomePage {
  @ViewChild("map")
  mapRef: ElementRef;
  @ViewChild("startPositionMarker")
  startPositionMarkerRef: ElementRef;
  @ViewChild("endPositionMarker")
  endPositionMarkerRef: ElementRef;
  uiComponentsVisible: boolean = true;
  filterByParameter: string;
  showStartPositionMarker: boolean = false;
  showEndPositionMarker: boolean = false;
  startPosition: google.maps.LatLng;
  endPostion: google.maps.LatLng;
  readyToGetHubValues: boolean = false;

  constructor(
    private modalCtrl: ModalController,
    private mapBuilder: MapBuilder,
    private hubProvider: HubProvider,
    private store$: Store<IHomePageState>,
    private alertsService: AlertsService,
    private apiProvider: ApiProvider,
    private actionSheetCtrl: ActionSheetController,
    private cd: ChangeDetectorRef
  ) {
    this.store$
      .pipe(select(getFilterByParameter))
      .subscribe((value: Parameters) => {
        this.filterByParameter = Parameters[value];
      });
    this.store$
      .pipe(select("homeState"))
      .subscribe((state: IHomePageState) => {
        if (state.lastAction.type == SET_SENSORS) {
          this.readyToGetHubValues = true;
        }
      });
  }
  ionViewDidLoad() {
    this.alertsService.showLoading("Инициализация приложения!");
    this.mapBuilder.onDrag(() => {
      this.hideUiComponents();
      this.cd.detectChanges();
    });
    this.mapBuilder.onDragEnd(() => {
      this.showUIComponents();
      this.cd.detectChanges();
    });
    this.mapBuilder.initMap(this.mapRef.nativeElement).then(() => {
      this.hubProvider.getHub().then(hub => {
        hub.on("DispatchReadingAsync", (hubDispatchModel: HubDispatchModel) => {
          if (!this.readyToGetHubValues) {
            return;
          }
          this.store$.dispatch(
            new UpdateSensorAction(
              hubDispatchModel.reading,
              hubDispatchModel.sensorId,
              hubDispatchModel.latestPollutionLevel
            )
          );
          this.mapBuilder.updateArea(
            hubDispatchModel.sensorId,
            hubDispatchModel.latestPollutionLevel,
            hubDispatchModel.reading[this.filterByParameter]
          );
        });
        hub.start().then(
          () => {
            this.apiProvider.getAllSensors().subscribe(response => {
              this.store$.dispatch(new SetSensorsAction(response));
              response.forEach(sensor => {
                this.mapBuilder.createNewArea(
                  sensor.id,
                  sensor.latestPollutionLevel,
                  sensor.latitude,
                  sensor.longitude,
                  sensor.readings[0][this.filterByParameter],
                  () => {
                    this.openDetailsModal(sensor.id);
                  }
                );
              });
              this.alertsService.hideLoading();
            });
          },
          error => this.alertsService.showError(error)
        );
      }, this.alertsService.showError);
    }, this.alertsService.showError);
  }

  openDetailsModal(sensorId: number) {
    const modal = this.modalCtrl.create(DetailsModal, { sensorId: sensorId });
    modal.present();
  }
  showParamsSheet() {
    let currentParameter = this.getState().filterByParameter;
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
            this.store$.dispatch(new SetFilterParameterAction(Parameters.cO2));
          }
        },
        {
          text: "LPG",
          cssClass: setClass(Parameters.lpg),
          handler: () => {
            this.store$.dispatch(new SetFilterParameterAction(Parameters.lpg));
          }
        },
        {
          text: "CO",
          cssClass: setClass(Parameters.co),
          handler: () => {
            this.store$.dispatch(new SetFilterParameterAction(Parameters.co));
          }
        },
        {
          text: "Пыль",
          cssClass: setClass(Parameters.dust),
          handler: () => {
            this.store$.dispatch(new SetFilterParameterAction(Parameters.dust));
          }
        }
      ]
    });
    actionSheet.present();
  }

  hideUiComponents() {
    if (this.uiComponentsVisible) {
      this.uiComponentsVisible = false;
    }
  }

  showUIComponents() {
    if (!this.uiComponentsVisible) {
      this.uiComponentsVisible = true;
    }
  }

  enableNavigationMode() {
    this.alertsService.showLoading();
    this.mapBuilder.enableNavigationMode().then(() => {
      this.showStartPositionMarker = true;
      this.alertsService.hideLoading();
      this.cd.detectChanges();
    }, this.alertsService.showError);
  }

  confirmNavigationPoint() {
    if (this.startPosition == null) {
      this.startPosition = this.mapBuilder.navigationModeSetAndGetStartPostionByPointCoordinates(
        this.startPositionMarkerRef.nativeElement.offsetLeft,
        this.startPositionMarkerRef.nativeElement.offsetTop
      );
      this.showStartPositionMarker = false;
      this.showEndPositionMarker = true;
      this.cd.detectChanges();
      return;
    }
    if (this.endPostion == null) {
      this.endPostion = this.mapBuilder.navigationModeSetAndGetEndPostionByPointCoordinates(
        this.endPositionMarkerRef.nativeElement.offsetLeft,
        this.endPositionMarkerRef.nativeElement.offsetTop
      );
      this.showEndPositionMarker = false;
      this.cd.detectChanges();
    }
    if (this.startPosition != null && this.endPostion != null) {
      this.mapBuilder.buildRoute(this.startPosition, this.endPostion);
      this.alertsService.showInfo(
        "Если вы хотите построить маршрут заново - просто выйдите и войдите в режим навигации снова!"
      );
    }
  }

  disableNavigationMode() {
    this.alertsService.showLoading();
    this.mapBuilder.disableNavigationMode().then(() => {
      this.showStartPositionMarker = false;
      this.showEndPositionMarker = false;
      this.startPosition = null;
      this.endPostion = null;
      this.alertsService.hideLoading();
      this.cd.detectChanges();
    }, this.alertsService.showError);
  }
  private getState(): IHomePageState {
    let state: IHomePageState;

    this.store$.take(1).subscribe(s => (state = s));

    return state;
  }
}
