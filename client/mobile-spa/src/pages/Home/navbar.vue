<template>
  <f7-navbar>
    <f7-nav-left>
      <f7-link class="panel-open"
               open-panel="left"
               icon="fa fa-bars"></f7-link>
    </f7-nav-left>
    <div class="title">Главная</div>
    <f7-nav-right>
      <f7-link v-show="showRouterSheetIcon"
               @click="$f7.sheet.open('.router-sheet')"
               icon="fa fa-road"></f7-link>
    </f7-nav-right>
  </f7-navbar>
</template>

<script>
export default {
  data() {
    return {
      showRouterSheetIcon: false
    };
  },
  mounted() {
    setTimeout(() => {
      let sheet = this.$f7.sheet.get(".router-sheet");
      this.showRouterSheetIcon = !sheet.opened;
      sheet.on("close", () => {
        this.showRouterSheetIcon = true;
      });
      sheet.on("open", () => {
        this.showRouterSheetIcon = false;
      });
    }, 0);
  }
};
</script>
