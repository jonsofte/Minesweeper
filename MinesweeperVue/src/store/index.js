import Vue from 'vue'
import Vuex from 'vuex'
import axios from 'axios'

Vue.use(Vuex)

export default new Vuex.Store({
  state: {
    gameConfiguration: {
      width: 12,
      height: 12,
      numberOfMines: 20
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
    }
  },
  actions: {
    setGameConfiguration: (context, configuration) => {
      context.commit('mutateGameConfiguration',configuration);
    },
    startNewGame(context) {
      console.log('Starting new game');
      // https://localhost:5001/game/newgame/
      axios.post('/game/newgame/',this.state.gameConfiguration)
        .then(
          (result) => {context.commit('setGameStatus',result.data) },
          (response) => { console.log(response) },
          (error) => { console.log(error) }
        );
    },
    exploreField(context, input) {
      console.log('Exploring field:', input.x , input.y);
      axios.post('/game/' +this.state.gameStatus.gameID,
      {
        ActionType: "Explore",
        X: input.x,
        Y: input.y
      })
      .then(
        (result) => {context.commit('setGameStatus',result.data) },
        (response) => { console.log(response) },
        (error) => { console.log(error) }
      );
    },
    flagField(context, input) {
      console.log('Flagging field:', input.x , input.y);
      axios.post('/game/' +this.state.gameStatus.gameID,
      {
        ActionType: "Flag",
        X: input.x,
        Y: input.y
      })
      .then(
        (result) => {context.commit('setGameStatus',result.data) },
        (response) => { console.log(response) },
        (error) => { console.log(error) }
      );
    },
    unflagField(context, input) {
      console.log('Unflagging field:', input.x , input.y);
      axios.post('/game/' +this.state.gameStatus.gameID,
      {
        ActionType: "Unflag",
        X: input.x,
        Y: input.y
      })
      .then(
        (result) => {context.commit('setGameStatus',result.data) },
        (response) => { console.log(response) },
        (error) => { console.log(error) }
      );
    },
    QuitGame(context) {
      console.log('Aborting game');
      axios.post('/game/' +this.state.gameStatus.gameID,
      {
        ActionType: "Quit",
      })
      .then(
        (result) => {context.commit('setGameStatus',result.data) },
        (response) => { console.log(response) },
        (error) => { console.log(error) }
      );
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
