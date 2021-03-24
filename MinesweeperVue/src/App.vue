<template>
  <v-app >
    <div class="app">
      <h1 class="text-h2 font-weight-bold text-center">Minesweeper</h1>
      <div v-if="displayConfiguration" class="mb-4">
        <GameConfiguration @aborted="onConfigAbort" @gameStart="onGameStart"/>
      </div>
        <v-container v-if="displayMinefield" justify="center" align="center">
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
      </v-container>
      <v-btn v-if="displayGameStartButton" @click="displayConfiguration = true" block>Start</v-btn>
    </div>
  </v-app>
</template>

<script>
import GameConfiguration from './components/GameConfiguration.vue';
import Tile from './components/Tile.vue';
import store from './store';

export default {
  name: 'App',
  store,
  components: {
    GameConfiguration,
    Tile
  },

  data: function() {
    return {
      displayConfiguration: false,
      displayMinefield : false,
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
    onConfigAbort() {
      this.displayConfiguration = false;
    },
    onGameStart(gameConfig) {
      this.displayConfiguration = false;
      this.displayMinefield = true;
      this.$store.dispatch('setGameConfiguration', gameConfig)
      console.log('Starting new game: Width:', gameConfig.width, "Height:", gameConfig.height, "NumberOfMines:", gameConfig.numberOfMines)
      this.$store.dispatch('startNewGame')
    },
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
    displayGameStartButton() {
      return !this.displayConfiguration && !this.displayMinefield;
    },
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