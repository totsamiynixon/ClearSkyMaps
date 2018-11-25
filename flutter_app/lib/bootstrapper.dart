import 'package:flutter_app/config/app_config.dart';
import 'package:flutter_app/infrastructure/service_locator.dart';
import 'package:flutter_app/services/impementations/apI_client_service_default.dart';
import 'package:flutter_app/services/implicits/api_service.dart';
import 'package:flutter_app/store/app_reducer.dart';
import 'package:flutter_app/store/app_state.dart';
import 'package:injector/injector.dart';
import 'package:redux/redux.dart';

class Bootstrapper {
  static void run() {
    var injector = Injector();
    injector.registerSingleton<AppConfig>((_) => AppConfig());
    injector.registerDependency<ApiClientService>(
        (inj) => ApiClientServiceDefault(inj.getDependency<AppConfig>()));
    injector.registerSingleton<Store<AppState>>((inj) =>
        new Store<AppState>(AppReducer.Execute, initialState: AppState()));
    ServiceLocator.current = injector;
  }
}
