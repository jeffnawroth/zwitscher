<template>
  <v-card
    title="Benutzer gesperrt"
    text="Ihr Benutzerkonto ist derzeit gesperrt. Bitte wenden Sie sich an den Administrator."
  >
    <v-card-actions>
      <v-spacer></v-spacer>
      <v-btn variant="tonal" @click="$emit('close')">Schließen</v-btn>
    </v-card-actions>
  </v-card>
</template>

<script setup lang="ts">
defineEmits(["close"]);
</script>
