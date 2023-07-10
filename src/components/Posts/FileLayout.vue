<template>
  <v-row v-if="files.length > 0">
    <v-col v-for="file in files" :key="JSON.stringify(file)" :cols="imgCols">
      <!-- Image -->
      <v-card
        v-if="
          (typeof file === 'string' && file?.includes('image')) ||
          //@ts-expect-error
          file?.type?.startsWith('image/')
        "
      >
        <v-img :src="generateFileURL(file)">
          <v-toolbar color="rgba(0, 0, 0, 0)" theme="dark">
            <template v-if="removeFileBtn" #prepend>
              <v-btn
                size="small"
                icon
                color="black"
                variant="tonal"
                @click="$emit('remove-file', file)"
              >
                <v-icon color="white">mdi-close</v-icon>
              </v-btn>
            </template>
          </v-toolbar>
        </v-img>
      </v-card>
      <!-- Video -->
      <div v-else class="video-player">
        <video ref="videoPlayer" controls>
          <source :src="generateFileURL(files[0])" type="video/mp4" />
        </video>
        <v-btn
          v-if="removeFileBtn"
          class="close-button"
          size="small"
          icon
          color="black"
          variant="tonal"
          @click="$emit('remove-file', file)"
        >
          <v-icon color="white">mdi-close</v-icon>
        </v-btn>
      </div>
    </v-col>
  </v-row>
</template>

<script setup lang="ts">
import { PropType, computed } from "vue";
import { generateFileURL } from "@/helpers";

defineEmits<{
  (e: "remove-file", file: File): void;
}>();

const props = defineProps({
  files: {
    type: Array as PropType<Array<File | string>>,
    default: () => {
      [];
    },
  },
  removeFileBtn: {
    type: Boolean,
  },
});

//Test
const imgCols = computed(() => {
  return props.files.length == 1 ? "12" : "6";
});
</script>

<style scoped>
.video-player {
  position: relative;
}

.close-button {
  position: absolute;
  top: 10px;
  left: 10px;
  z-index: 1;
}
</style>
