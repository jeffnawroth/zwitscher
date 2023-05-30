<template>
  <v-toolbar density="compact" color="white">
    <v-toolbar-title class="font-weight-bold">{{ title }}</v-toolbar-title>
    <slot></slot>
  </v-toolbar>
</template>

<script setup lang="ts">
defineProps({
  title: {
    type: String,
    required: true,
  },
});
</script>
