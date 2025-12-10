<script setup lang="ts">
import { computed } from 'vue'
import { useUsersStore } from '@/store/users'

const store = useUsersStore()
const lockButtonText = computed(() => {
  return store.user?.locked ? 'Entsperren' : 'Sperren'
})

const lockIcon = computed(() => {
  return store.user?.locked ? 'mdi-lock-open-outline' : 'mdi-lock-outline'
})
</script>

<template>
  <v-btn variant="tonal" color="orange" :prepend-icon="lockIcon">
    {{ lockButtonText }}
  </v-btn>
</template>
