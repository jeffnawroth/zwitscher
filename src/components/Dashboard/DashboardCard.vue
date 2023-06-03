<template>
  <v-card :title="title">
    <v-card-text>
      <slot></slot>
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
  title: {
    type: String,
    required: true,
  },
});
defineEmits(["download"]);
</script>
