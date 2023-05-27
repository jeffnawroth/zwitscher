<template>
  <BaseDeleteDialog
    :model-value="modelValue"
    @cancel="closeDialog"
    @delete="removeUser"
  >
    den Nutzer
    {{ `'${store.user?.name}' (${store.user?.username})` }}
  </BaseDeleteDialog>
</template>

<script lang="ts" setup>
import { useUsersStore } from "@/store/users";
import BaseDeleteDialog from "./BaseComponents/BaseDeleteDialog.vue";
import router from "@/router";

const store = useUsersStore();

defineProps({
  modelValue: {
    type: Boolean,
  },
});

const emit = defineEmits(["update:modelValue"]);

function closeDialog() {
  emit("update:modelValue", false);
}

function removeUser() {
  store.deleteUser();
  closeDialog();
  router.push({ name: "home" });
}
</script>
