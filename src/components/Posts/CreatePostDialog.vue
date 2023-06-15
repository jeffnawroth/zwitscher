<template>
  <v-dialog
    :model-value="modelValue"
    width="500"
    @update:model-value="$emit('update:model-value', $event)"
  >
    <CreatePost></CreatePost>
  </v-dialog>
</template>

<script setup lang="ts">
import CreatePost from "./CreatePost.vue";

defineEmits(["update:model-value"]);

defineProps({
  modelValue: {
    type: Boolean,
  },
});
</script>
