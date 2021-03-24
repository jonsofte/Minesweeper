<template>
  <v-container>
    <v-row >
      <v-col justify="center" align="center">
        <div v-for="y in height" :key="y-1">
          <Tile v-for="x in width" :key="x-1" 
              :tileValue="getTile(x,y)" 
              :xValue="x-1" 
              :yValue="y-1" 
              @explored="exploreField"
              @flagged="flagField"
              @unflagged="unflagField"
          />
        </div>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>

import Tile from './Tile.vue';
import store from '../store';

export default {
  name: 'Minefield',
  store,
  components: {
    Tile
  },

  data: function() {
    return {
      tileStates: [
        {state: "Hidden", icon: false, value:"", color:"", flat:false},
        {state: "Empty", icon: false, value:"", color:"blue-grey lighten-4", flat:true},
        {state: "One", icon: false, value:"1", color:"blue-grey lighten-4", flat:true},
        {state: "Two", icon: false, value:"2", color:"blue-grey lighten-4", flat:true},
        {state: "Three", icon: false, value:"3", color:"blue-grey lighten-4", flat:true},
        {state: "Four",icon: false, value:"4", color:"blue-grey lighten-4", flat:true},
        {state: "Five",icon: false, value:"5", color:"blue-grey lighten-4", flat:true},
        {state: "Six",icon: false, value:"6", color:"blue-grey lighten-4", flat:true},
        {state: "Seven",icon: false, value:"7", color:"blue-grey lighten-4", flat:true},
        {state: "Eight",icon: false, value:"8", color:"blue-grey lighten-4", flat:true},
        {state: "Explosion",icon: true, value:"mdi-nuke", color:"red darken-2", flat:true},
        {state: "Flagged",icon: true, value:"mdi-flag-variant", color:"green lighten-4", flat:false},
        {state: "Mine",icon: true, value:"mdi-mine", color:"red lighten-3", flat:true}
          ],
      }
  },
  methods: {
    exploreField(xValue ,yValue) {
      console.log("Exploring:", xValue, yValue)
      this.$store.dispatch('exploreField', { x: xValue, y: yValue })
    },
    unflagField(xValue ,yValue) {
      console.log("Unflag:", xValue, yValue)
      this.$store.dispatch('unflagField', { x: xValue, y: yValue })
    },
    flagField(xValue ,yValue) {
      console.log("Flag:", xValue, yValue)
      this.$store.dispatch('flagField', { x: xValue, y: yValue })
    },
    getTile(x,y) {
      return this.$store.getters.getMinefield[((this.width*(y-1)+x)-1)];
    }
  },
  computed:  {
    width() {
      return this.$store.getters.getGameConfiguration.width;
    },
    height() {
      return this.$store.getters.getGameConfiguration.height;
    },
    numberOfTiles() {
      return this.width*this.height;
    }
  }
}

</script>

<style>
</style>