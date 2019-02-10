import { Component, ElementRef, ViewChild } from '@angular/core';
import { IAppState } from 'src/app/state/app.reducer';
import { Store } from '@ngrx/store';
import { SetSensors, InitMap } from 'src/app/state/home/home.actions';
import { MapService } from 'src/app/core/services/map.service';

@Component({
  selector: 'app-home',
  templateUrl: 'home.page.html',
  styleUrls: ['home.page.scss'],
})
export class HomePage {
  @ViewChild("map")
  mapRef: ElementRef;

  constructor(private store$: Store<IAppState>, private mapService: MapService) {
  }

  ionViewDidLoad() {
    this.store$.dispatch(new InitMap(this.mapRef.nativeElement));
  }
}
