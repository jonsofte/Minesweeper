<template>
  <v-container class="minefieldTiles">
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
    <v-row class="text-center font-weight-light mb-3">
      <v-col>
        Time used: <b>{{time}}</b> seconds. Number of moves: <b>{{numberOfMoves}}</b>. Flags used: <b>{{flagsUsed}}/{{numberOfMines}}</b><br>
        Fields explored: <b>{{fieldsExplored}}/{{numberOfTiles}}</b>. Mines: <b>{{numberOfMines}}</b>
      </v-col>
    </v-row>
    <v-row align="center" justify="center" class="text-center font-weight-light mb-3">
      <v-col lg=2 md=4 sm=6>
        <v-btn @click="endGame" block>{{ButtonEndGameText}}</v-btn>
      </v-col>
    </v-row>
    <v-row align="center" justify="center" class="text-center text-h5 mb-3">
      <v-col lg=6 md=8 sm=12>
        {{GameStatusMessage}}
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
      time: 0,
      interval: null
      }
  },
  methods: {
    exploreField(xValue ,yValue) {
      if (this.interval == null) this.startTimer();
      console.log("Exploring:", xValue, yValue);
      this.$store.dispatch('exploreField', { x: xValue, y: yValue });
    },
    unflagField(xValue ,yValue) {
      console.log("Unflag:", xValue, yValue);
      this.$store.dispatch('unflagField', { x: xValue, y: yValue });
    },
    flagField(xValue ,yValue) {
      console.log("Flag:", xValue, yValue);
      this.$store.dispatch('flagField', { x: xValue, y: yValue });
    },
    getTile(x,y) {
      return this.$store.getters.getMinefield[((this.width*(y-1)+x)-1)];
    },
    endGame() {
      this.stopTimer();
      if (this.$store.getters.getGameStatus.gameStatus === "Active") this.$store.dispatch('quitGame');
      this.$store.dispatch('resetMinefield');
      this.$emit('gameEnded');
    },
    startTimer() {
      this.interval = setInterval(this.incrementTime, 1000);
    },
    stopTimer() {
      clearInterval(this.interval);
    },
    incrementTime() {
      this.time = parseInt(this.time) + 1;
    },
    storeGameHistoryInLocalStorage() {
      var previousGames = [];
      if (localStorage.getItem('gameHistory')) {
        try {
          previousGames = JSON.parse(localStorage.getItem('gameHistory'));
        } catch(e) {
          localStorage.removeItem('gameHistory');
        }
      }
      if (previousGames.length >= 10) previousGames.pop()
      previousGames.unshift(
        {
          id: this.$store.getters.getGameStatus.gameID, 
          result: this.getGameStatusText(this.$store.getters.getGameStatus.gameStatus), 
          startTime: this.$store.getters.getGameStatus.gameStartedTime, 
          secondsUsed: this.time, 
          moves: this.numberOfMoves, 
          flags: this.flagsUsed, 
          minefield: 
            {
              width: this.width, 
              height: this.height,
              mines: this.numberOfMines
            }  
        }
      );
      const parsed = JSON.stringify(previousGames);
      localStorage.setItem('gameHistory', parsed);
    },
    getGameStatusText(input) {
      if (input == "EndedFailed") return "Failed";
      if (input == "EndedSuccess") return "Completed";
      return "";
    }
  },
  computed:  {
    width() {
      return this.$store.getters.getGameConfiguration.width;
    },
    height() {
      return this.$store.getters.getGameConfiguration.height;
    },
    numberOfMines() {
      return this.$store.getters.getGameConfiguration.numberOfMines;
    },
    numberOfTiles() {
      return this.width*this.height;
    },
    numberOfMoves() {
      return this.$store.getters.getGameStatus.numberOfMoves;
    },
    fieldsExplored() {
      return this.$store.getters.getGameStatus.numberOfFieldsExplored;
    },
    flagsUsed() {
      return this.$store.getters.getGameStatus.numberOfFlagsUsed;
    },
    ButtonEndGameText() {
      if (this.$store.getters.getGameStatus.gameStatus === "Active") return "Abort Game";
      return "Main Menu";
    },
    GameStatusMessage() {
      if (this.$store.getters.getGameStatus.gameStatus === "EndedSuccess") {
        this.stopTimer();
        this.storeGameHistoryInLocalStorage();
        return "Congratulations! Game completed in "+ this.time +" seconds with "+this.numberOfMoves+" moves";
      }
      if (this.$store.getters.getGameStatus.gameStatus === "EndedFailed") {
        this.stopTimer();
        this.storeGameHistoryInLocalStorage();
        return "BOOM! Game Over!";
      }
      return "";
    }
  }
}

</script>
<style>

/*
.minefieldTiles .bd:hover {
  animation-name: boom;
  animation-duration: 0.07s;
  animation-iteration-count: 7;
  animation-fill-mode: forwards;
}

@-webkit-keyframes boom {
  from { top: 0; left: 0; animation-timing-function: linear; }
  20% { top: -3x; left: -10px; animation-timing-function: linear; }
  40% { top: 2px; left: -10px; animation-timing-function: linear; }
  60% { top: 2px; left: 10px; animation-timing-function: linear; }
  80% { top: -2px; left: -10px; animation-timing-function: linear; }
  to { top: 0; left: 0; animation-timing-function: linear; }
}

*/
</style>