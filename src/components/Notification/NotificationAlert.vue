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

<script setup lang="ts">
import { NotificationAlert } from "@/interfaces";
import { onBeforeUnmount } from "vue";
import { PropType } from "vue";
import { onMounted } from "vue";
import { ref } from "vue";
import { useNotificationStore } from "@/store/notification";

const props = defineProps({
  notification: {
    type: Object as PropType<NotificationAlert>,
    required: true,
  },
});

const store = useNotificationStore();
const timeout = ref({});
const interval = ref({});
const alert = ref(true);
const progress = ref(0);

onMounted(() => {
  timeout.value = setTimeout(() => {
    store.deleteNotification(props.notification);
  }, 6000);
  interval.value = setInterval(() => {
    if (progress.value === 100) {
      return (progress.value = 0);
    }
    progress.value += 20;
  }, 1000);
});

onBeforeUnmount(() => {
  clearTimeout(timeout.value as number);
  clearInterval(interval.value as number);
});
</script>

<style scoped>
#alert {
  z-index: 10000;
}
</style>
