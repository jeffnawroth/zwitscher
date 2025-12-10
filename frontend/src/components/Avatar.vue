<script lang="ts" setup>
import { generateFileURL } from '@/helpers'

defineProps({
  file: {
    type: [File, String],
    default: () => null,
  },
})
</script>

<template>
  <v-btn icon size="100">
    <v-avatar size="100">
      <v-img v-if="file" cover :src="generateFileURL(file)" />
      <v-icon v-else size="x-large">
        mdi-camera-plus-outline
      </v-icon>
    </v-avatar>
  </v-btn>
</template>
