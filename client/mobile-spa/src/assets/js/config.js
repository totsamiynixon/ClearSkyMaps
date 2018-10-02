const config = {
  baseUrl: null,
  hubName: "readingsHub"
};

if (process.env.NODE_ENV == "production") {
  config.baseUrl = "production_url";
} else {
  config.baseUrl = "http://localhost:51545/";
}

export default config;
