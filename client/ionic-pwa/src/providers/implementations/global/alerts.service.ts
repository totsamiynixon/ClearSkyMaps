import { AlertsService } from "../../inerfaces/global/alerts.service";
import { ToastController, LoadingController, Loading } from "ionic-angular";
import { Injectable } from "@angular/core";
@Injectable()
export class IonicAlertsService implements AlertsService {
  loadingState: boolean = false;
  loading: Loading;
  constructor(
    public toastCtrl: ToastController,
    public loadingCtrl: LoadingController
  ) {}

  showLoading(message?: string): void {
    this.loading = this.loadingCtrl.create({
      content: message || "Подождите, пожалуйста"
    });
    this.loadingState = true;
    setTimeout(() => {
      if (this.loadingState) {
        this.loading.present();
      }
    }, 300);
  }
  hideLoading(): void {
    this.loadingState = false;
    setTimeout(() => {
      if (!this.loadingState && this.loading != null) {
        this.loading.dismiss();
        this.loading = null;
      }
    }, 200);
  }
  showError(error?: Error): void {
    let message = "Что-то пошло не так!";
    if (error != null && error.message) {
      message = error.message;
    }
    let toast = this.toastCtrl.create({
      message: message,
      duration: 3000,
      position: "bottom"
    });
    toast.present();
  }
}
