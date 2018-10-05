<template>
  <f7-sheet class="router-sheet"
            :class="{'desktop':$device.desktop}"
            :opened="true"
            :closeByBackdropClick="false"
            :backdrop="false"
            @sheet:closed="sheetOpened = false">
    <f7-toolbar>
      <div class="left"></div>
      <div class="right"
           v-show="true || !$device.desktop">
        <f7-link sheet-close>Закрыть</f7-link>
      </div>
    </f7-toolbar>
    <!-- Scrollable sheet content -->
    <f7-page-content>
      <f7-list class="router-list"
               :class="{'desktop':$device.desktop}">
        <f7-list-item>
          <f7-label floating>Откуда</f7-label>
          <f7-input type="text"
                    placeholder="Откуда" />
        </f7-list-item>
        <f7-list-item>
          <f7-label floating>Куда</f7-label>
          <f7-input type="text"
                    placeholder="Куда" />
        </f7-list-item>
      </f7-list>
      <!-- ... -->
    </f7-page-content>
  </f7-sheet>
</template>

<script>
export default {
  data() {
    return {
      sheetOpened: false
    };
  }
  //props: ["sheetOpened", "from", "to"]
};
</script>

<style scoped>
.router-sheet.desktop {
  height: 178px;
}
.router-list.desktop {
  margin: 0;
}
</style>
