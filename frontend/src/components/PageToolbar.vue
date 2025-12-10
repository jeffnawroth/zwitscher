<script setup lang="ts">
import { useRouter } from 'vue-router'

defineProps({
  title: {
    type: String,
    required: true,
  },
  backButton: {
    type: Boolean,
  },
  icon: {
    type: String,
    required: true,
  },
})

const router = useRouter()
</script>

<template>
  <v-toolbar density="compact">
    <template #prepend>
      <v-btn
        v-if="backButton"
        variant="plain"
        icon="mdi-arrow-left"
        @click="router.back()"
      />
      <v-icon>{{ icon }}</v-icon>
    </template>

    <!-- <v-app-bar-nav-icon></v-app-bar-nav-icon> -->
    <v-toolbar-title class="font-weight-bold">
      {{ title }}
    </v-toolbar-title>
    <slot />
  </v-toolbar>
</template>
