import { Component, ElementRef, ViewChild } from '@angular/core';
import { IAppState } from 'src/app/state/app.reducer';
import { Store } from '@ngrx/store';
import { InitMap } from 'src/app/state/home/home.actions';

@Component({
  selector: 'app-home',
  templateUrl: 'home.page.html',
  styleUrls: ['home.page.scss'],
})
export class HomePage {
  @ViewChild("map")
  mapRef: ElementRef;

  constructor(private store$: Store<IAppState>) {
  }

  ngOnInit() {
    this.store$.dispatch(new InitMap(this.mapRef.nativeElement));
  }
}
