<template>
  <f7-navbar>
    <f7-nav-left>
      <f7-link class="panel-open"
               open-panel="left"
               icon-material="menu"></f7-link>
    </f7-nav-left>
    <div class="title">Главная</div>
    <f7-nav-right>
      <f7-link v-show="!routerSheetOpened"
               @click="handleOpenRouterSheetClick()"
               icon-material="directions"></f7-link>
    </f7-nav-right>
  </f7-navbar>
</template>

<script>
export default {
  props: ["routerSheetOpened"],
  mounted() {},
  methods: {
    handleOpenRouterSheetClick() {
      this.$emit("routerSheetOpened");
    }
  }
};
</script>
