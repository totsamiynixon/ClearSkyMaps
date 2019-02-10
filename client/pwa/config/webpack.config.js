const path = require("path");
const fs = require("fs");
const { dev, prod } = require("@ionic/app-scripts/config/webpack.config");
const webpackMerge = require("webpack-merge");
const webpack = require("webpack");
const customConfig = {};
process.env.NODE_ENV = process.env.NODE_ENV || "dev";
const state = {
  APP_WEBSERVER_URL: JSON.stringify(
    process.env[process.env.NODE_ENV.toUpperCase() + "_API_URL"] ||
    "http://localhost:56875/"
  ),
  VERSION: JSON.stringify(require("../package.json").version),
  APP_MAP_API_KEY: JSON.stringify(
    process.env.APP_MAP_API_KEY || "AIzaSyAfj-ARjZc7VEGb0_grdk5VFu5wXphQyjo"
  )
};
console.log("CONSOLE HELLO!");
initWebpackConfig();
module.exports = {
  dev: webpackMerge(dev, customConfig),
  prod: webpackMerge(prod, customConfig)
};

function initWebpackConfig() {
  let newDefinePlugin = new webpack.DefinePlugin(state);
  customConfig.plugins = [newDefinePlugin];
  let mergedPlugin = webpackMerge.smart(
    dev.plugins[0],
    customConfig.plugins[0]
  );
  dev.plugins.shift();
  prod.plugins.shift();
}