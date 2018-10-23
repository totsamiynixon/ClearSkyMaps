const path = require("path");
const { dev, prod } = require("@ionic/app-scripts/config/webpack.config");
const webpackMerge = require("webpack-merge");
const webpack = require("webpack");
const newDefinePlugin = new webpack.DefinePlugin({
  APP_WEBSERVER_URL: JSON.stringify(
    process.env["APP_WEBSERVER_URL_" + process.env.APP_STAGE] ||
      "http://localhost:56875/"
  ),
  VERSION: JSON.stringify(require("../package.json").version),
  APP_MAP_API_KEY: JSON.stringify(
    process.env.APP_MAP_API_KEY || "AIzaSyAfj-ARjZc7VEGb0_grdk5VFu5wXphQyjo"
  )
});
const customConfig = {
  plugins: [newDefinePlugin]
};
const mergedPlugin = webpackMerge.smart(
  dev.plugins[0],
  customConfig.plugins[0]
);
console.log("I USE MY CUSTOM CONFIG");
console.log(mergedPlugin);
dev.plugins.shift();
prod.plugins.shift();
console.log("Dev conf after merge", webpackMerge(dev, customConfig));
console.log("Prod conf after merge", webpackMerge(dev, customConfig));
module.exports = {
  dev: webpackMerge(dev, customConfig),
  prod: webpackMerge(prod, customConfig)
};
