
export interface IConfig {
  hubPath: string;
  applicationServerUrl: string;
  map: IConfigMap;
}

export interface IConfigMap {
  key: string;
  options: any;
}