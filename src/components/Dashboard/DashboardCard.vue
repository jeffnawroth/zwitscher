<template>
  <v-card :title="title" :loading="loading">
    <v-card-text>
      <canvas :id="id"></canvas>
    </v-card-text>
    <v-card-actions>
      <v-spacer></v-spacer>
      <v-btn prepend-icon="mdi-download" @click="$emit('download')"
        >Download</v-btn
      >
    </v-card-actions>
  </v-card>
</template>

<script lang="ts" setup>
defineProps({
  id: {
    type: String,
    required: true,
  },
  title: {
    type: String,
    required: true,
  },
  loading: {
    type: Boolean,
  },
});
defineEmits(["download"]);
</script>
