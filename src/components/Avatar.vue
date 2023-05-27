<template>
  <v-btn icon size="100">
    <v-img>
      <v-avatar
        :size="100"
        class="avatar"
        :image="generateFileURL(file)"
      ></v-avatar>
    </v-img>
  </v-btn>
</template>

<script lang="ts" setup>
import { generateFileURL } from "@/helpers";
import { PropType } from "vue";

defineProps({
  file: {
    type: Object as PropType<File>,
    default: () => {
      null;
    },
  },
});
</script>
