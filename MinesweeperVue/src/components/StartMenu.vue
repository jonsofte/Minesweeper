<template>
  <div>
    <h1 class="text-h2 font-weight-bold text-center">
      <v-icon x-large color="green darken-2">mdi-flag-variant</v-icon>Minesweeper<v-icon x-large color="green darken-2">mdi-flag-variant</v-icon>
    </h1>
    <v-container>
      <v-row align="center" justify="center">
        <v-col lg=4 md=5 sm=10>
          <br>
          <v-btn @click="startNewGame" block>Start New Game</v-btn>
          <br>
          <p class="text-center font-weight-light">
            <b>Mouse:</b><br>
            Left Button: Explore<br>
            Right Button: Flag/Unflag<br>
            <br><b>Tablet/Phone:</b><br>
            Tap: Explore<br>
            Hold for one second: Flag/Unflag<br><br>
            Source code at: <a href="https://github.com/jonsofte/Minesweeper">https://github.com/jonsofte/Minesweeper</a>
          </p>
        </v-col>
      </v-row>
    </v-container>
    <GameLog />
  </div>
</template>

<script>
import GameLog from './GameLog.vue';

export default {
  name: 'StartMenu',
  components: {
    GameLog,
  },
  props: {},
  computed: {},
  watch: {},
  methods: {
    startNewGame() {
      this.$emit('newGame')
    }
  },
  data: function() {
    return {}
  },
}
</script>

<style>
</style>