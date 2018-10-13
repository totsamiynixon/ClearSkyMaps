import { Config, ConfigMap } from "../../../models/providers/config";

export class ConfigProvider {
  development: Config;
  staging: Config;
  production: Config;
  constructor() {
    this.buildDevConfig();
    this.buildStagingConfig();
    this.buildProductionConfig();
  }

  buildDevConfig() {
    this.development = new Config();
    this.development.hubPath = "/readingsHub";
    this.development.map = new ConfigMap();
  }
}
