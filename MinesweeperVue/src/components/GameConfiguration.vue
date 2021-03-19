<template>
  <v-container class="grey lighten-5 body-1">
    <!-- <v-layout row wrap> -->
      <v-row><v-spacer/>
        <v-col class="text-right col-2">Width:</v-col><v-col class="col-4"><v-slider max="100" min="0" v-model="config.width"/></v-col><v-col  class="col-2">{{ config.width }}</v-col>
      <v-spacer/></v-row>
      <v-row><v-spacer/>
        <v-col class="text-right col-2">Height:</v-col><v-col class="col-4"><v-slider max="100" min="0" v-model="config.height"/></v-col><v-col class=" col-2">{{ config.height }}</v-col>
      <v-spacer/></v-row> 
      <v-row><v-spacer/>
        <v-col class="text-right col-2">Number Of Mines: </v-col><v-col class="col-4"><v-slider :max="maxNumberOfMines" min="0" v-model="config.numberOfMines"/></v-col>
        <v-col class="col-2">{{ config.numberOfMines }} of {{ maxNumberOfMines}} tiles</v-col>
      <v-spacer/></v-row>
      <v-row>
        <v-spacer/>
        <v-col class="col-3"><v-btn @click="onStartButton" block>Start</v-btn></v-col>
        <v-col class="col-3"><v-btn @click="onAbortButton" block>Abort</v-btn></v-col>
        <v-spacer/>
      </v-row>
    <!-- </v-layout> -->
  </v-container>
</template>

<script>
import store from '../store';

export default {
  name: 'GameConfiguration',
  store,
  component: {},
  props: {},
  data: function() {
    return {
      config: {
        // width: 12,
        // height: 12,
        // numberOfMines: 20
        width: 0,
        height: 0,
        numberOfMines: 0
      }
    }
  },
  computed: {
    maxNumberOfMines() {
      return this.config.width*this.config.height;
    }
  },
  watch: {},
  methods: {
    onStartButton() {
      this.$emit('gameStart', this.config)
    },
    onAbortButton() {
      this.$emit('aborted')
    }
  },
  beforeMount() {
    this.config = this.$store.getters.getGameConfiguration;
  }
}
</script>

<style>
</style>