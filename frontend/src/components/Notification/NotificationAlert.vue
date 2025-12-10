<script setup lang="ts">
import type { PropType } from 'vue'
import type { NotificationAlert } from '@/interfaces'
import { onBeforeUnmount, onMounted, ref } from 'vue'
import { useNotificationStore } from '@/store/notification'

const props = defineProps({
  notification: {
    type: Object as PropType<NotificationAlert>,
    required: true,
  },
})

const store = useNotificationStore()
const timeout = ref({})
const interval = ref({})
const alert = ref(true)
const progress = ref(0)

onMounted(() => {
  // Delete notification after 4 seconds
  timeout.value = setTimeout(() => {
    store.deleteNotification(props.notification)
  }, 4000)

  // Set circular progress
  interval.value = setInterval(() => {
    if (progress.value === 100) {
      return (progress.value = 0)
    }
    progress.value += 100 / 3
  }, 1000)
})

onBeforeUnmount(() => {
  clearTimeout(timeout.value as number)
  clearInterval(interval.value as number)
})
</script>

<template>
  <v-alert
    id="alert"
    v-model="alert"
    :text="notification.text"
    :type="notification.type"
    max-width="400"
    closable
    border="start"
  >
    <template #close>
      <v-progress-circular :model-value="progress">
        <v-icon size="x-small" @click="alert = false">
          mdi-window-close
        </v-icon>
      </v-progress-circular>
    </template>
  </v-alert>
</template>

<style scoped>
#alert {
  z-index: 10000;
}
</style>
