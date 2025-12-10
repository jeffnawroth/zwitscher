<script setup lang="ts">
import { useField } from 'vee-validate'
import { toRef } from 'vue'

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
})

const { value, handleBlur, errors } = useField(toRef(props, 'name'), undefined)
</script>

<template>
  <v-select
    v-model="value"
    :label="label"
    :error-messages="errors"
    :clearable="true"
    :items="items"
    item-value="value"
    item-title="text"
    hide-details="auto"
    @blur="handleBlur"
  />
</template>
