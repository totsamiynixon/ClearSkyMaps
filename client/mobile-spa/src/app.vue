<template>
  <f7-app :params="f7params">
    <f7-statusbar></f7-statusbar>
    <f7-view url="/"
             :main="true"
             class="ios-edges"></f7-view>
  </f7-app>
</template>

<script>
// Import Routes
import routes from "./routes.js";

export default {
  data() {
    return {
      // Framework7 parameters here
      f7params: {
        id: "io.framework7.testapp", // App bundle ID
        name: "Framework7", // App name
        theme: "auto", // Automatic theme detection
        // App routes
        routes: routes
      }
    };
  }
};
</script>
