<template>
  <v-dialog
    :model-value="modelValue"
    max-width="500"
    persistent
    @input="$emit('update:modelValue', $event)"
  >
    <v-card title="Löschen" prepend-icon="mdi-delete">
      <v-card-text>
        Sind Sie sicher, dass Sie <slot></slot> löschen möchten?
      </v-card-text>
      <v-card-actions>
        <v-spacer></v-spacer>
        <v-btn variant="tonal" @click="$emit('cancel')">Abbrechen</v-btn>
        <v-btn
          variant="tonal"
          color="red"
          prepend-icon="mdi-delete-outline"
          @click="$emit('delete')"
          >Löschen</v-btn
        >
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
defineEmits(["cancel", "delete", "update:modelValue"]);
defineProps({
  modelValue: {
    type: Boolean,
  },
});
</script>
