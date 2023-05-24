<template>
  <v-textarea
    v-model="value"
    clearable
    hide-details="auto"
    :error-messages="errors"
    auto-grow
    @blur="handleBlur"
  ></v-textarea>
</template>

<script lang="ts" setup>
import { useField } from "vee-validate";
import { toRef } from "vue";

const props = defineProps({
  name: {
    type: String,
    required: true,
  },
  type: {
    type: String,
    required: true,
  },
});

const { value, handleBlur, errors } = useField(toRef(props, "name"), undefined);
</script>
