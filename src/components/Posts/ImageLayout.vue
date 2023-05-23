<template>
  <v-row v-if="files.length > 0">
    <v-col v-for="file in files" :key="file.name" :cols="imgCols">
      <v-card>
        <v-img cover aspect-ratio="1/1" :src="previewImage(file)">
          <v-toolbar color="rgba(0, 0, 0, 0)" theme="dark">
            <template v-if="deleteImgBtn" #prepend>
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
    </v-col>
  </v-row>
</template>

<script setup lang="ts">
import { PropType, computed } from "vue";

defineEmits<{
  (e: "remove-file", file: File): void;
}>();

const props = defineProps({
  files: {
    type: Array as PropType<Array<File>>,
    default: () => {
      [];
    },
  },
  deleteImgBtn: {
    type: Boolean,
  },
});

function previewImage(file: File) {
  return URL.createObjectURL(file);
}

//Test
const imgCols = computed(() => {
  return props.files.length == 1 ? "12" : "6";
});
</script>
