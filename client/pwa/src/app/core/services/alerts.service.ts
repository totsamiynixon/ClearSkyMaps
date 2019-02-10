import { Injectable } from "@angular/core";
import { ToastController } from "@ionic/angular";
@Injectable({
  providedIn: 'root',
})
export class AlertsService {
  constructor(
    public toastCtrl: ToastController
  ) { }

  async showError(error?: Error) {
    let message = "Что-то пошло не так!";
    if (error != null && error.message) {
      message = error.message;
    }
    let toast = await this.toastCtrl.create({
      message: message,
      duration: 3000,
      position: "bottom",
      showCloseButton: true
    });
    toast.present();
  }
  async  showInfo(message: string) {
    let toast = await this.toastCtrl.create({
      message: message,
      duration: 8000,
      position: "bottom"
    });
    toast.present();
  }
}
