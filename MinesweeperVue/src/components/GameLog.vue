<template>
  <div>
    <h1 class="text-h5 text-center" v-if="gameHistory.length > 0">
      History
    </h1>
    <v-container>
      <v-row align="center" justify="center">
        <v-col lg=8 md=10 sm=12>
          <v-card tile v-for="game in gameHistory" :key="game.id" class="text-center pa-0 ma-0">
            <v-layout wrap :class="'pa-1 gamestatus ' + game.result">
              <v-row>
                <v-col sm=3 md=3 lg=3>
                  <div class="caption grey--text">Time</div>
                  <div>{{formatDateTime(game.startTime)}}</div>
                </v-col>
                <v-col sm=2 md=2 lg=2>
                  <div class="caption grey--text">Seconds</div>
                  <div>{{game.secondsUsed}}</div>
                </v-col>
                <v-col sm=2 md=1 lg=1>
                  <div class="caption grey--text">Moves</div>
                  <div>{{game.moves}}</div>
                </v-col>
                <v-col sm=2 md=1 lg=1>
                  <div class="caption grey--text">Flags</div>
                  <div>{{game.flags}}</div>
                </v-col>
                <v-col sm=3 md=3 lg=3>
                  <div class="caption grey--text">Minefield</div>
                  <div>{{game.minefield.width}}x{{game.minefield.height}} - {{game.minefield.mines}} mines</div>
                </v-col>
                <v-col sm=2 md=2 lg=2>
                  <div class="caption grey--text">Status</div>
                  <div>{{game.result}}</div>
                </v-col>
              </v-row>
            </v-layout>
          </v-card>
        </v-col>
      </v-row>
    </v-container>
  </div>
</template>

<script>

export default {
  name: 'GameLog',
  component: {},
  props: {},
  computed: {},
  watch: {},
  methods: {
    getColorClass(resultText) {
      if (resultText == "Failed") return ".gamestatus.failed";
      if (resultText == "Completed") return ".gamestatus.completed";
      return "";
    },
    pad(number) {
     return (number < 10 ? '0' : '') + number
    },
    formatDateTime(datetime) {
      var m = new Date(datetime);
      return m.getFullYear() +"-"+ (m.getMonth()+1) +"-"+ m.getDate() + " " + this.pad(m.getHours()) + ":" + this.pad(m.getMinutes()) + ":" + this.pad(m.getSeconds());
    },
  },
  data: function() {
    return {
      gameHistory: []
    }
  },
  mounted() {
    if (localStorage.getItem('gameHistory')) {
      try {
        this.gameHistory = JSON.parse(localStorage.getItem('gameHistory'));
      } catch(e) {
        localStorage.removeItem('gameHistory');
      }
    }
  }
}

</script>

<style>

.gamestatus.Completed {
  border-left: 10px solid rgb(143, 216, 143);
}
.gamestatus.Failed {
  border-left: 10px solid rgb(231, 96, 72);
}

</style>