<template>
  <v-btn variant="tonal" color="orange" :prepend-icon="lockIcon">
    {{ lockButtonText }}
  </v-btn>
</template>

<script setup lang="ts">
import { useUsersStore } from "@/store/users";
import { computed } from "vue";

const store = useUsersStore();
const lockButtonText = computed(() => {
  return store.user?.locked ? "Entsperren" : "Sperren";
});

const lockIcon = computed(() => {
  return store.user?.locked ? "mdi-lock-open-outline" : "mdi-lock-outline";
});
</script>
