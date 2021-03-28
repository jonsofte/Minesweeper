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
    }
  },
  mutations: {
    mutateGameConfiguration: (state, configuration) => {
      state.gameConfiguration = configuration;
    },
    setGameStatus: (state,game) => {
      state.gameStatus = game;
    },
    resetGameGridData: (state) => {
      state.gameStatus.minefield.gridData = [];
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
          (error) => { console.log(error) }
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
        (error) => { console.log(error) }
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
        (error) => { console.log(error) }
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
        (error) => { console.log(error) }
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
          context.commit('resetGameGridData');
        },
        // (response) => { },
        (error) => { console.log(error) }
      );
    },
    resetMinefield(context) {
      context.commit('resetGameGridData');
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
    }
  }
})
