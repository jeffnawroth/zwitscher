<script setup lang="ts">
defineOptions({
  inheritAttrs: false,
})
defineProps({
  icon: {
    type: String,
    required: true,
  },
  text: {
    type: String,
    required: true,
  },
})
</script>

<template>
  <v-tooltip location="bottom" :text="text">
    <template #activator="{ props }">
      <v-icon v-bind="{ ...props, ...$attrs }" class="me-2">
        {{ icon }}
      </v-icon>
    </template>
  </v-tooltip>
</template>
