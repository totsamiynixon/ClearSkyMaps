import { tap, switchMap, map } from "rxjs/operators";
import { Injectable } from "@angular/core";
import { Actions, Effect, ofType } from "@ngrx/effects";

import { Observable } from "rxjs";
import { SignalRHubService } from "src/app/core/services/hub.service";
import { HomePageActionTypes, UpdateSensor, SetSensors, SetSensorsSuccess, InitMap } from "./home.actions";
import { IAppState } from "../app.reducer";
import { Store } from "@ngrx/store";
import { HubDispatchModel } from "src/app/core/models/dispatchReading,model";
import { AlertsService } from "src/app/core/services/alerts.service";
import { MapService } from "src/app/core/services/map.service";
import { ModalController } from "@ionic/angular";
import { DetailsModal } from "src/app/pages/home/details/details";
import { ApiService } from "src/app/core/services/api.service";
import { Sensor } from "src/app/core/models/sensor.model";

@Injectable()
export class HomePageEffects {
  constructor(
    private actions: Actions,
    private hubService: SignalRHubService,
    private alertsService: AlertsService,
    private apiService: ApiService,
    private store$: Store<IAppState>,
    private mapService: MapService,
    private modalCtrl: ModalController
  ) { }


  @Effect({ dispatch: false })
  initMap: Observable<any> = this.actions.pipe(
    ofType(HomePageActionTypes.INIT_MAP),
    tap((action: InitMap) => {
      this.mapService.initMap(action.element).then(() => {
        this.store$.dispatch(new SetSensors);
      }, this.alertsService.showError)
    }
    ));

  @Effect()
  setSensors: Observable<any> = this.actions.pipe(
    ofType(HomePageActionTypes.SET_SENSORS),
    switchMap(() => {
      return this.apiService.getAllSensors().pipe(
        map((sensors: Sensor[]) => {
          return new SetSensorsSuccess(sensors);
        })
      )
    }
    ));

  @Effect({ dispatch: false })
  InitHub: Observable<any> = this.actions.pipe(
    ofType(HomePageActionTypes.SET_SENSORS_SUCCESS),
    tap((action: SetSensorsSuccess) => {
      if (action.payload) {
        action.payload.forEach(sensor => {
          this.mapService.createNewArea(
            sensor.id,
            sensor.latestPollutionLevel,
            sensor.latitude,
            sensor.longitude,
            async () => {
              const modal = await this.modalCtrl.create({
                component: DetailsModal,
                componentProps: { sensorId: sensor.id }
              });
              modal.present();
            }
          );
        })
      }
      this.hubService.getHub().then(hub => {
        hub.on("DispatchReadingAsync", (hubDispatchModel: HubDispatchModel) => {
          this.store$.dispatch(
            new UpdateSensor(
              hubDispatchModel.reading,
              hubDispatchModel.sensorId,
              hubDispatchModel.latestPollutionLevel
            )
          )
        });
        hub.start().then(null, this.alertsService.showError);
      }, this.alertsService.showError);
    })
  );

  @Effect({ dispatch: false })
  UpdateSensor: Observable<any> = this.actions.pipe(
    ofType(HomePageActionTypes.UPDATE_SENSOR),
    tap((action: UpdateSensor) => {
      this.mapService.updateArea(
        action.sensorId,
        action.pollutionLevel
      );
    })
  );
}
