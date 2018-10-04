// Import Vue
import Vue from "vue";

// Import F7
import Framework7 from "framework7/framework7.esm.bundle.js";

// Import F7 Vue Plugin
import Framework7Vue from "framework7-vue/framework7-vue.esm.bundle.js";

// Import F7 Styles
import Framework7Styles from "framework7/css/framework7.css";

// Import Icons and App Custom Styles
import IconsStyles from "./css/icons.css";
import AppStyles from "./css/app.css";

import MaterialIcons from "material-design-icons/iconfont/material-icons.css";
import FontAwesome from "font-awesome/css/font-awesome.css";
// Import App Component
import App from "./app";

// Init F7 Vue Plugin
Framework7.use(Framework7Vue);

// Init App
new Vue({
  el: "#app",
  template: "<app/>",

  // Register App Component
  components: {
    app: App
  }
});
