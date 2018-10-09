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
      <f7-list-item v-show="expanded">
        <f7-label floating>{{currentProp == "from" ? "Откуда" : "Куда"}}</f7-label>
        <f7-input type="text"
                  @input="handleQueryChanged"
                  :value="this.query"
                  ref="query" />
      </f7-list-item>
      <f7-list-item v-show="!expanded">
        <f7-label floating>Откуда</f7-label>
        <f7-input type="text"
                  @focus="handleFocus('from')"
                  :value="this.waypoints.from.address" />
      </f7-list-item>
      <f7-list-item v-show="!expanded">
        <f7-label floating>Куда</f7-label>
        <f7-input type="text"
                  @focus="handleFocus('to')"
                  :value="this.waypoints.to.address" />
      </f7-list-item>
    </f7-list>
    <f7-list class="router-list"
             v-show="expanded">
      <f7-list-item v-for="proposition in propositions"
                    :key="proposition.id"
                    @click="handlePlaceSelected(proposition)">
        {{proposition.address}}
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
      query: ""
    };
  },
  methods: {
    handleFocus(prop) {
      this.expanded = true;
      this.query = this.waypoints[prop].address;
      this.currentProp = prop;
      this.$refs.query.$el.children[0].focus();
    },
    handlePlaceSelected(selectedPlace) {
      this.expanded = false;
      this.query = "";
      this.$emit("placeSelected", {
        selectedPlace,
        propName: this.currentProp
      });
    },
    closeRouterSheet() {
      this.$emit("closed");
    },
    handleQueryChanged(event) {
      this.query = event.target.value;
      this.$emit("queryChanged", this.query);
    }
  },
  props: ["opened", "propositions", "waypoints"]
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
