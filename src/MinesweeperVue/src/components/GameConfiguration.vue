<template>
  <div class="mb-3">
    <v-container class="lighten-5 body-1 font-weight-light" fill-height fluid >
      <br>
      <v-row align="center" justify="center">
        <v-col lg=4 md=6 sm=8 class="text-center text-h6">Select Minefield:</v-col>
      </v-row>
      <v-row align="center" justify="center">
        <v-col lg=1 md=1 sm=2 class="text-right">Width:</v-col>
        <v-col lg=3 md=3 sm=8><v-slider max="35" min="0" v-model="config.width"/></v-col>
        <v-col lg=1 md=1 sm=2>{{ config.width }}</v-col>
      </v-row>
      <v-row align="center" justify="center">
        <v-col lg=1 md=1 sm=2 class="text-right">Height:</v-col>
        <v-col lg=3 md=3 sm=8><v-slider max="50" min="0" v-model="config.height"/></v-col>
        <v-col lg=1 md=1 sm=2>{{ config.height }}</v-col>
      </v-row> 
      <v-row align="center" justify="center">
        <v-col lg=1 md=1 sm=2 class="text-right">Number Of Mines: </v-col>
        <v-col lg=3 md=3 sm=8><v-slider :max="maxNumberOfMines" min="0" v-model="config.numberOfMines"/></v-col>
        <v-col lg=1 md=1 sm=2>{{ config.numberOfMines }} of {{ maxNumberOfMines}} tiles</v-col>
      </v-row>
      <v-row align="center" justify="center">
        <v-col lg=2 md=2 sm=4><v-btn @click="onStartButton" block>Start</v-btn></v-col>
        <v-col lg=2 md=2 sm=4><v-btn @click="onAbortButton" block>Abort</v-btn></v-col>
      </v-row>
    </v-container>
  </div>
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