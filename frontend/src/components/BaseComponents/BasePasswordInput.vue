<script setup lang="ts">
import { ref } from 'vue'
import BaseInputWithValidation from './BaseInputWithValidation.vue'

defineProps({
  name: {
    type: String,
    required: true,
  },

  label: {
    type: String,
    required: true,
  },
})

const showPassword = ref(false)
</script>

<template>
  <BaseInputWithValidation
    :name="name"
    :label="label"
    :type="showPassword ? 'text' : 'password'"
    :append-inner-icon="showPassword ? 'mdi-eye' : 'mdi-eye-off'"
    @click:append-inner="showPassword = !showPassword"
  />
</template>
