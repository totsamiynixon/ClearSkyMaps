const path = require("path");
const fs = require("fs");
const { dev, prod } = require("@ionic/app-scripts/config/webpack.config");
const webpackMerge = require("webpack-merge");
const webpack = require("webpack");
const customConfig = {};
const state = {
  APP_WEBSERVER_URL: JSON.stringify(
    process.env["APP_WEBSERVER_URL_" + process.env.APP_STAGE] ||
      "http://localhost:56875/"
  ),
  VERSION: JSON.stringify(require("../package.json").version),
  APP_MAP_API_KEY: JSON.stringify(
    process.env.APP_MAP_API_KEY || "AIzaSyAfj-ARjZc7VEGb0_grdk5VFu5wXphQyjo"
  )
};
initConfigXML();
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
function initConfigXML() {
  const replaceAll = function(target, search, replacement) {
    return target.split(search).join(replacement);
  };
  let file = fs.readFileSync(path.join(__dirname, "../config.tpl.xml"), "utf8");
  for (var key in state) {
    file = replaceAll(file, `"$(${key})"`, state[key]);
  }
  fs.writeFileSync(path.join(__dirname, "../config.xml"), file, "utf8");
}
