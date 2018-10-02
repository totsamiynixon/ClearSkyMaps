import axios from "axios";
import config from "app-config";

axios.defaults.baseURL = config.baseURL;

export const getAllSensors = () => {
  return axios.get("/api/sensors");
};
