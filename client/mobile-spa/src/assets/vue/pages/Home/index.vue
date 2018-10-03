<template>
  <f7-page>
    <home-sidebar></home-sidebar>
    <home-navbar></home-navbar>
    <f7-button style="margin-bottom:1em; text-transform: capitalize;"
               fill
               round
               raised
               @click="openPopup">
      Открыть попап
    </f7-button>
    <home-details-popup></home-details-popup>
  </f7-page>
</template>

<script>
import HomeSidebar from "./sidebar";
import HomeNavbar from "./navbar";
import HomeDetailsPopup from "./details.modal";
export default {
  data() {
    return {};
  },
  methods: {
    openPopup() {
      console.log("clicked");
      this.$f7.popup.open(document.getElementById("home-details-popup"));
    }
  },
  components: {
    HomeSidebar,
    HomeNavbar,
    HomeDetailsPopup
  }
};
</script>
