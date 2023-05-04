<template>
  <BaseInputWithValidation
    :name="name"
    :label="label"
    :type="showPassword ? 'text' : 'password'"
    :append-inner-icon="showPassword ? 'mdi-eye' : 'mdi-eye-off'"
    @click:append-inner="showPassword = !showPassword"
  ></BaseInputWithValidation>
</template>

<script setup lang="ts">
import { ref } from "vue";
import BaseInputWithValidation from "./BaseInputWithValidation.vue";

const showPassword = ref(false);

defineProps({
  name: {
    type: String,
    required: true,
  },

  label: {
    type: String,
    required: true,
  },
});
</script>
