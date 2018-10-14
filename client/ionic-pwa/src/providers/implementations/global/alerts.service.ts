import { AlertsService } from "../../inerfaces/global/alerts.service";
import { ToastController } from "ionic-angular";
import { Injectable } from "@angular/core";
@Injectable()
export class ToastAlertsService implements AlertsService {
  constructor(public toastCtrl: ToastController) {}
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
