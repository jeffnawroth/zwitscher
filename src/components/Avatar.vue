<template>
  <v-btn icon size="100">
    <v-avatar class="avatar" size="100">
      <v-img v-if="file" cover :src="generateFileURL(file)"></v-img>
      <v-icon v-else size="x-large">mdi-camera-plus-outline</v-icon>
    </v-avatar>
  </v-btn>
</template>

<script lang="ts" setup>
import { generateFileURL } from "@/helpers";
import { PropType } from "vue";

defineProps({
  file: {
    type: Object as PropType<File>,
    default: () => {
      null;
    },
  },
});
</script>
