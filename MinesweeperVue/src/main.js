import Vue from 'vue'
import App from './App.vue'
import store from './store'
import vuetify from './plugins/vuetify';
import VueAppInsights from 'vue-application-insights'

Vue.config.productionTip = false

Vue.use(VueAppInsights, {
  id: process.env.VUE_APP_APPLICATION_INSIGHTS_KEY
})

new Vue({
  store,
  vuetify,
  render: h => h(App)
}).$mount('#app')
