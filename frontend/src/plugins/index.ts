/**
 * plugins/index.ts
 *
 * Automatically included in `./src/main.ts`
 */

// Types
import type { App } from 'vue'
import router from '../router'
import pinia from '../store'
import vuetify from './vuetify'

// Plugins
import { loadFonts } from './webfontloader'

export function registerPlugins(app: App) {
  loadFonts()
  app
    .use(vuetify)
    .use(router)
    .use(pinia)
}
