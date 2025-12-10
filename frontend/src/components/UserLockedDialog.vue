<script setup lang="ts">
defineProps({
  modelValue: {
    type: Boolean,
  },
})
defineEmits(['close', 'update:model-value'])
</script>

<template>
  <v-dialog
    :model-value="modelValue"
    width="auto"
    @update:model-value="$emit('update:model-value', $event)"
  >
    <v-card
      title="Benutzer gesperrt"
      text="Ihr Benutzerkonto ist derzeit gesperrt. Bitte wenden Sie sich an den Administrator."
      width="500"
    >
      <v-card-actions>
        <v-spacer />
        <v-btn variant="tonal" @click="$emit('close')">
          Schließen
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>
