import 'package:injector/injector.dart';

class ServiceLocator {
  static Injector current;
  static T getService<T>() {
    if (current != null) {
      return current.getDependency<T>();
    }
    throw new NullThrownError();
  }
}
