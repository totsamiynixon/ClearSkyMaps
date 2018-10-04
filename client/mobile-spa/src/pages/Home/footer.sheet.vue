<template>
  <f7-sheet class="router-sheet"
            :opened="true"
            :closeByBackdropClick="false"
            :backdrop="false"
            @sheet:closed="sheetOpened = false">
    <f7-toolbar>
      <div class="left"></div>
      <div class="right"
           v-show="!$theme.desktop">
        <f7-link sheet-close>Close</f7-link>
      </div>
    </f7-toolbar>
    <!-- Scrollable sheet content -->
    <f7-page-content>
      <f7-block>

        <!-- ... -->
      </f7-block>
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
  //props:["sheetOpened", "from", "to"]
};
</script>
