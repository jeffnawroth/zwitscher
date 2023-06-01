<template>
  <v-toolbar density="compact" color="white">
    <v-btn
      v-if="backButton"
      variant="plain"
      icon="mdi-chevron-left"
      @click="router.back()"
    ></v-btn>
    <v-toolbar-title class="font-weight-bold">{{ title }}</v-toolbar-title>
    <slot></slot>
  </v-toolbar>
</template>

<script setup lang="ts">
import { useRouter } from "vue-router";
defineProps({
  title: {
    type: String,
    required: true,
  },
  backButton: {
    type: Boolean,
  },
});

const router = useRouter();
</script>
