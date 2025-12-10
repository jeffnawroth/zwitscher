<script lang="ts" setup>
import { useRoute, useRouter } from 'vue-router'
import { useUsersStore } from '@/store/users'
import BaseDeleteDialog from './BaseComponents/BaseDeleteDialog.vue'

defineProps({
  modelValue: {
    type: Boolean,
  },
})
const emit = defineEmits(['update:modelValue'])
const store = useUsersStore()
const router = useRouter()
const route = useRoute()

function closeDialog() {
  emit('update:modelValue', false)
}

// Delete the user
async function removeUser() {
  await store.deleteUser(store.user!.id!)
  closeDialog()
  if (route.name === 'profile')
    router.push({ name: 'home' })
}
</script>

<template>
  <BaseDeleteDialog
    :model-value="modelValue"
    :loading="store.crudCardLoading"
    @cancel="closeDialog"
    @delete="removeUser"
  >
    den Nutzer
    {{ `'${store.user?.name}' (${store.user?.username})` }}
  </BaseDeleteDialog>
</template>
