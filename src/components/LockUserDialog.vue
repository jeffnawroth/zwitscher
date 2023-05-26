<template>
  <v-dialog
    :model-value="modelValue"
    max-width="500"
    persistent
    @input="$emit('update:modelValue', $event)"
  >
    <v-card
      class="mx-auto"
      :prepend-icon="lockCardTitleIcon"
      :title="lockCardTitle"
    >
      <v-card-text>
        Sind Sie sicher, dass Sie den Nutzer
        {{ userDisplayName }}
        {{ lockCardTextAction }} möchten?
      </v-card-text>
      <v-card-actions>
        <v-spacer></v-spacer>
        <v-btn variant="tonal" @click="closeDialog">Abbrechen</v-btn>
        <LockButton @click="toggleUserLock"></LockButton>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script lang="ts" setup>
import { useUsersStore } from "@/store/users";
import { computed } from "vue";
import LockButton from "./LockButton.vue";

const store = useUsersStore();

defineProps({
  modelValue: {
    type: Boolean,
  },
});

const emit = defineEmits(["update:modelValue"]);

const lockCardTitle = computed(() => {
  return store.user?.locked ? "Entsperren" : "Sperren";
});

const lockCardTextAction = computed(() => {
  return store.user?.locked ? "entsperren" : "sperren";
});

const lockCardTitleIcon = computed(() => {
  return store.user?.locked ? "mdi-lock-open" : "mdi-lock";
});

const userDisplayName = computed(() => {
  return `${store.user?.firstName} ${store.user?.lastName} (${store.user?.username})`;
});

function toggleUserLock() {
  store.user!.locked! = !store.user!.locked!;
  closeDialog();
}

function closeDialog() {
  emit("update:modelValue", false);
}
</script>
