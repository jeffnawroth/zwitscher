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
import { useRouter, useRoute } from "vue-router";

const store = useUsersStore();
const router = useRouter();
const route = useRoute();

defineProps({
  modelValue: {
    type: Boolean,
  },
});

const emit = defineEmits(["update:modelValue"]);

function closeDialog() {
  emit("update:modelValue", false);
}

async function removeUser() {
  await store.deleteUser(store.user!.id!);
  closeDialog();
  if (route.name === "profile") router.push({ name: "home" });
}
</script>
