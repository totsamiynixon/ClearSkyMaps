import { Injectable } from "@angular/core";
import { ToastController } from "@ionic/angular";
@Injectable({
  providedIn: 'root',
})
export class AlertsService {
  constructor(
    public toastCtrl: ToastController
  ) {
  }

  showError(error?: Error) {
    let message = "Что-то пошло не так!";
    if (error != null && error.message) {
      message = error.message;
    }
    this.toastCtrl.create({
      message: message,
      duration: 3000,
      position: "bottom",
      showCloseButton: true
    }).then((toast) => toast.present());
  }
  showInfo(message: string) {
    this.toastCtrl.create({
      message: message,
      duration: 8000,
      position: "bottom"
    }).then((toast) => toast.present());
  }
}
