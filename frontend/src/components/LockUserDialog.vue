<script lang="ts" setup>
import { computed } from 'vue'
import { useUsersStore } from '@/store/users'
import LockButton from './LockButton.vue'

defineProps({
  modelValue: {
    type: Boolean,
  },
})

const emit = defineEmits(['update:modelValue'])

const store = useUsersStore()

const lockCardTitle = computed(() => {
  return store.user?.locked ? 'Entsperren' : 'Sperren'
})

const lockCardTextAction = computed(() => {
  return store.user?.locked ? 'entsperren' : 'sperren'
})

const lockCardTitleIcon = computed(() => {
  return store.user?.locked ? 'mdi-lock-open' : 'mdi-lock'
})

const userDisplayName = computed(() => {
  return `${store.user?.name} (${store.user?.username})`
})

/**
 * Lock or unlock the user
 */
async function toggleUserLock() {
  store.user!.locked! = !store.user!.locked!
  await store.updateUser(store.user!)
  closeDialog()
}

function closeDialog() {
  emit('update:modelValue', false)
}
</script>

<template>
  <v-dialog
    :model-value="modelValue"
    max-width="500"
    persistent
    @update:model-value="$emit('update:modelValue', $event)"
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
        <v-spacer />
        <v-btn variant="tonal" @click="closeDialog">
          Abbrechen
        </v-btn>
        <LockButton @click="toggleUserLock" />
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>
