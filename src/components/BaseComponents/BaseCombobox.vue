<template>
  <v-combobox
    v-model="value"
    :label="label"
    clearable
    :items="items"
    item-title="value"
    multiple
    chips
    closable-chips
    hide-details="auto"
    :error-messages="errors"
    @blur="handleBlur"
  >
  </v-combobox>
</template>

<script setup lang="ts">
import { toRef } from "vue";
import { useField } from "vee-validate";

const props = defineProps({
  name: {
    type: String,
    required: true,
  },

  label: {
    type: String,
    required: true,
  },

  items: {
    type: Array,
    required: true,
  },
});

const { value, handleBlur, errors } = useField<unknown[]>(toRef(props, "name"));
</script>
