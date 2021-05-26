<template>
  <v-app >
    <div class="app">
      <StartMenu v-if="displayStartMenu" @newGame="onNewGame" />
      <GameConfiguration v-if="displayConfiguration" @aborted="onGotoStartMenu" @gameStart="onGameStart"/>
      <Minefield v-if="displayMinefield" @gameEnded="onGotoStartMenu"/>
    </div>
  </v-app>
</template>

<script>

import StartMenu from './components/StartMenu.vue';
import GameConfiguration from './components/GameConfiguration.vue';
import Minefield from './components/Minefield.vue';
import store from './store';

export default {
  name: 'App',
  store,
  components: {
    StartMenu,
    GameConfiguration,
    Minefield,
  },

  data: function() {
    return {
      displayStartMenu: true,
      displayConfiguration: false,
      displayMinefield : false,
      }
  },
  methods: {
    onNewGame() {
      this.displayStartMenu = false;
      this.displayConfiguration = true;
    },
    onGotoStartMenu() {
      this.displayConfiguration = false;
      this.displayStartMenu = true;
      this.displayMinefield = false;
    },
    onGameStart(gameConfig) {
      this.displayConfiguration = false;
      this.displayMinefield = true;
      this.$store.dispatch('setGameConfiguration', gameConfig);
      this.$store.dispatch('startNewGame');
    }
  },
  computed:  {
    displayGameStartButton() {
      return !this.displayConfiguration && !this.displayMinefield;
    }
  }
}

</script>