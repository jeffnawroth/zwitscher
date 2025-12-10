<script setup lang="ts">
import CreatePost from './CreatePost.vue'

defineProps({
  modelValue: {
    type: Boolean,
  },
})

defineEmits(['update:model-value'])
</script>

<template>
  <v-dialog
    :model-value="modelValue"
    width="500"
    @update:model-value="$emit('update:model-value', $event)"
  >
    <CreatePost @close-dialog="$emit('update:model-value', false)" />
  </v-dialog>
</template>
