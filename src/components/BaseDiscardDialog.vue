<template>
  <v-dialog
    :model-value="modelValue"
    max-width="500"
    persistent
    @input="$emit('update:modelValue', $event)"
  >
    <v-card>
      <v-card-title>Änderungen verwerfen</v-card-title>
      <v-card-text>
        Sind Sie sicher, dass Sie Ihre Änderungen verwerfen möchten?
      </v-card-text>
      <v-card-actions>
        <v-spacer></v-spacer>
        <v-btn @click="$emit('cancel')">Abbrechen</v-btn>
        <v-btn @click="$emit('discard')">Verwerfen</v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
defineEmits(["cancel", "discard", "update:modelValue"]);
defineProps({
  modelValue: {
    type: Boolean,
  },
});
</script>
