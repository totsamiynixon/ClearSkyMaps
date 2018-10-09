import axios from "axios";
import config from "@/config";

axios.defaults.baseURL = "/";

const getAllSensors = () => {
  return axios.get("api/sensors");
};

export default {
  getAllSensors
};
