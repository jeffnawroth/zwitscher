<template>
  <v-text-field
    v-model="value"
    :label="label"
    :error-messages="errors"
    :type="type"
    :clearable="true"
    hide-details="auto"
    @blur="handleBlur"
  />
</template>

<script setup lang="ts">
import { toRef } from "vue";
import { useField } from "vee-validate";

const props = defineProps({
  name: {
    type: String,
    required: true,
  },
  type: {
    type: String,
    required: true,
  },
  label: {
    type: String,
    required: true,
  },
});

const { value, handleBlur, errors } = useField(toRef(props, "name"), undefined);
</script>
