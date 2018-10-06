<template>
  <f7-page class="router-sheet"
           :class="{'expanded': expanded, 'closed': !opened}">
    <f7-navbar>
      <div class="left"></div>
      <div class="right"
           v-show="expanded">
        <f7-link @click="expanded = !expanded">Свернуть</f7-link>
      </div>
      <div class="right"
           v-show="!expanded">
        <f7-link @click="closeRouterSheet()">Закрыть</f7-link>
      </div>
    </f7-navbar>
    <f7-list class="router-inputs">
      <f7-list-item v-show="!expanded || currentProp == 'from'">
        <f7-label floating>Откуда</f7-label>
        <f7-input type="text"
                  @focus="handleFocus('from')" />
      </f7-list-item>
      <f7-list-item v-show="!expanded || currentProp == 'to'">
        <f7-label floating>Куда</f7-label>
        <f7-input type="text"
                  @focus="handleFocus('to')" />
      </f7-list-item>
    </f7-list>
    <f7-list class="router-list"
             v-show="expanded">
      <f7-list-item @click="handleBlur">
        <span>1232312</span>
      </f7-list-item>
    </f7-list>
  </f7-page>
</template>

<script>
export default {
  data() {
    return {
      expanded: false,
      currentProp: "from",
      propositions: []
    };
  },
  methods: {
    handleFocus(prop) {
      this.expanded = true;
      this.currentProp = prop;
    },
    handleBlur() {
      this.expanded = false;
    },
    closeRouterSheet() {
      this.$emit("closed");
    }
  },
  props: ["opened"]
};
</script>

<style scoped>
.router-sheet {
  position: fixed;
  overflow: hidden;
  bottom: 0;
  top: initial;
  transition: height 0.2s ease-in, bottom 0.1s ease-out;
  z-index: 5001;
}

.ios .router-sheet {
  height: 172px;
}
.md .router-sheet {
  height: 186px;
}
.device-desktop .router-sheet {
  height: 194px;
}
.router-sheet.expanded {
  transition: height 0.2s ease-out;
}
.device-desktop .router-sheet.expanded {
  height: 600px;
}
.router-sheet.expanded {
  height: 100%;
}
.router-sheet.closed {
  transition: bottom 0.1s ease-in;
}
.ios .router-sheet.closed {
  bottom: -172px;
}
.md .router-sheet.closed {
  bottom: -186px;
}
.device-desktop .router-sheet.closed {
  bottom: -178px;
}
.router-inputs {
  margin: 0;
}
.router-inputs ul:before,
.router-inputs ul:after {
  content: none !important;
}
</style>
