<script setup lang="ts">
defineProps({
  modelValue: {
    type: Boolean,
  },
})
defineEmits(['cancel', 'discard', 'update:modelValue'])
</script>

<template>
  <v-dialog
    :model-value="modelValue"
    max-width="500"
    persistent
    @update:model-value="$emit('update:modelValue', $event)"
  >
    <v-card>
      <v-card-title>Änderungen verwerfen</v-card-title>
      <v-card-text>
        Sind Sie sicher, dass Sie Ihre Änderungen verwerfen möchten?
      </v-card-text>
      <v-card-actions>
        <v-spacer />
        <v-btn variant="tonal" @click="$emit('cancel')">
          Abbrechen
        </v-btn>
        <v-btn variant="tonal" @click="$emit('discard')">
          Verwerfen
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>
