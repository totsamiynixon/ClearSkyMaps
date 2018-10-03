<template>
  <f7-popup id="home-details-popup">
    <f7-page :page-content="false">
      <f7-navbar>
        <f7-nav-left>
          <f7-link popup-close
                   icon="icon-back"></f7-link>
        </f7-nav-left>
        <f7-nav-title>
          Tabbar
        </f7-nav-title>
        <f7-nav-right v-if="$theme.md">
          <f7-link icon-material="compare_arrows"></f7-link>
        </f7-nav-right>
      </f7-navbar>
      <f7-toolbar tabbar>
        <f7-link tab-link="#tab-1"
                 tab-link-active>График</f7-link>
        <f7-link tab-link="#tab-2">Таблица</f7-link>
      </f7-toolbar>
      <f7-tabs>
        <f7-tab id="tab-1"
                class="page-content"
                tab-active>
          <f7-block>
            <p>График здесь</p>
            ...
          </f7-block>
        </f7-tab>
        <f7-tab id="tab-2"
                class="page-content">
          <f7-block>
            <p>Таблица здесь</p>
            ...
          </f7-block>
        </f7-tab>
      </f7-tabs>
    </f7-page>
  </f7-popup>
</template>
<script>
export default {};
</script>
