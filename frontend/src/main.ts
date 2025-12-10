/**
 * main.ts
 *
 * Bootstraps Vuetify and other plugins then mounts the App`
 */

import axios from 'axios'

// Composables
import { createApp } from 'vue'

// Plugins
import { registerPlugins } from '@/plugins'

// Components
import App from './App.vue'
import { useAuthenticationStore } from './store/authentication'

/**
 * Try to refresh user token and try axios call again when user is unauthorized
 */
axios.interceptors.response.use(
  response => response,
  async (error) => {
    const originalConfig = error.config
    if (error.response.status === 401 && !originalConfig._retry) {
      originalConfig._retry = true
      const store = useAuthenticationStore()
      try {
        await store.refreshUserToken({
          token: store.user?.token || '',
          refreshToken: store.user?.refreshToken || '',
        })
        originalConfig.headers.Authorization = `Bearer ${store.user?.token}`
        return await axios(originalConfig)
      }
      catch (error) {
        return Promise.reject(error)
      }
    }
    return Promise.reject(error)
  },
)

const app = createApp(App)

registerPlugins(app)

app.mount('#app')
