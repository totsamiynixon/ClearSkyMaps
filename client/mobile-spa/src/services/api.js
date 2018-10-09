import axios from "axios";
import config from "@/config";

axios.defaults.baseURL = config.baseUrl;

const getAllSensors = () => {
  return axios.get("sensors");
};

export default {
  getAllSensors
};
