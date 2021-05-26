import Vue from 'vue'
import Vuex from 'vuex'
import axios from 'axios'

Vue.use(Vuex)

export default new Vuex.Store({
  state: {
    gameConfiguration: {
      width: 16,
      height: 16,
      numberOfMines: 30
    },
    gameStatus: {
      gameID: null,
      gameStatus: null,
      gameStartedTime: null,
      gameMoves: [],
      numberOfMoves: null,
      numberOfFieldsExplored: null,
      numberOfFlagsUsed: null,
      minefield: {
        numberOfFields: 0,
        width: 0,
        height: 0,
        numberOfMines: 0,
        gridData: [],
        fieldTypeValues: {}
      }
    },
    serverError: null,
  },
  mutations: {
    mutateGameConfiguration: (state, configuration) => {
      state.gameConfiguration = configuration;
    },
    setGameStatus: (state,game) => {
      state.gameStatus = game;
    },
    resetGame: (state) => {
      state.gameStatus.minefield.gridData = [];
      state.gameStatus.gameStatus = "Uninitialized";
    },
    setError: (state, error) => {
      state.serverError = error;
    }
  },
  actions: {
    setGameConfiguration: (context, configuration) => {
      context.commit('mutateGameConfiguration',configuration);
    },
    startNewGame(context) {
      axios.post(process.env.VUE_APP_API_URL + 'game/newgame/',this.state.gameConfiguration)
        .then(
          (result) => {context.commit('setGameStatus',result.data) },
          // (response) => { },
          (error) => { context.commit ('setError', error) }
        );
    },
    exploreField(context, input) {
      axios.post(process.env.VUE_APP_API_URL + 'game/' +this.state.gameStatus.gameID,
      {
        ActionType: "Explore",
        X: input.x,
        Y: input.y
      })
      .then(
        (result) => {context.commit('setGameStatus',result.data) },
        // (response) => { },
        (error) => { context.commit ('setError', error) }
      );
    },
    flagField(context, input) {
      axios.post(process.env.VUE_APP_API_URL + 'game/' +this.state.gameStatus.gameID,
      {
        ActionType: "Flag",
        X: input.x,
        Y: input.y
      })
      .then(
        (result) => {context.commit('setGameStatus',result.data) },
        // (response) => { },
        (error) => { context.commit ('setError', error) }
      );
    },
    unflagField(context, input) {
      axios.post(process.env.VUE_APP_API_URL + 'game/' +this.state.gameStatus.gameID,
      {
        ActionType: "Unflag",
        X: input.x,
        Y: input.y
      })
      .then(
        (result) => {context.commit('setGameStatus',result.data) },
        // (response) => { },
        (error) => { context.commit ('setError', error) }
      );
    },
    quitGame(context) {
      axios.post(process.env.VUE_APP_API_URL + 'game/' +this.state.gameStatus.gameID,
      {
        ActionType: "Quit",
      })
      .then(
        (result) => {
          context.commit('setGameStatus',result.data);
          context.commit('resetGame');
        },
        // (response) => { },
        (error) => { context.commit ('setError', error) }
      );
    },
    resetMinefield(context) {
      context.commit('resetGame');
    }
  },
  modules: {
  },
  getters: {
    getGameConfiguration: state => {
      return state.gameConfiguration
    },
    getGameStatus: state => {
      return state.gameStatus
    },
    getGameID: state => {
      return state.gameStatus.gameID
    },
    getMinefield: state => {
      return state.gameStatus.minefield.gridData
    },
    getError: state => {
      return state.serverError
    }
  }
})
