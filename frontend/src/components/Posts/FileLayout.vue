<script setup lang="ts">
import type { PropType } from 'vue'
import { computed } from 'vue'
import { generateFileURL } from '@/helpers'

const props = defineProps({
  files: {
    type: Array as PropType<Array<File | string>>,
    default: () => [],
  },
  removeFileBtn: {
    type: Boolean,
  },
})

defineEmits<{
  (e: 'removeFile', file: File): void
}>()

const imgCols = computed(() => {
  return props.files.length === 1 ? '12' : '6'
})
</script>

<template>
  <v-row v-if="files.length > 0">
    <v-col v-for="file in files" :key="JSON.stringify(file)" :cols="imgCols">
      <!-- Image -->
      <v-card
        v-if="
          (typeof file === 'string' && file?.includes('image'))
            //@ts-expect-error
            || file?.type?.startsWith('image/')
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
                @click="$emit('removeFile', file as File)"
              >
                <v-icon color="white">
                  mdi-close
                </v-icon>
              </v-btn>
            </template>
          </v-toolbar>
        </v-img>
      </v-card>
      <!-- Video -->
      <div v-else class="video-player">
        <video controls>
          <source :src="generateFileURL(files[0])" type="video/mp4">
        </video>
        <v-btn
          v-if="removeFileBtn"
          class="close-button"
          size="small"
          icon
          color="black"
          variant="tonal"
          @click="$emit('removeFile', file as File)"
        >
          <v-icon color="white">
            mdi-close
          </v-icon>
        </v-btn>
      </div>
    </v-col>
  </v-row>
</template>

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
