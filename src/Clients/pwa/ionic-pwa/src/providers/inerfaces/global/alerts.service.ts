import { Injectable } from "@angular/core";
import { ToastController } from "ionic-angular";
@Injectable()
export abstract class AlertsService {
  abstract showError(error?: Error): void;
  abstract showInfo(message: string): void;
  abstract showLoading(message?: string): void;
  abstract hideLoading(): void;
}
