import axios from "axios";
import config from "app-config";

axios.defaults.baseURL = config.baseURL;

getAllSensors = () => {
  return axios.get("/api/sensors");
};

export default {
  getAllSensors
};
