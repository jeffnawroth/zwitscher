<template>
  <v-row>
    <v-col>
      <v-select
        v-model="day"
        hide-details="auto"
        label="Tag"
        :items="days"
      ></v-select>
    </v-col>
    <v-col>
      <v-select
        v-model="month"
        hide-details="auto"
        label="Monat"
        :items="months"
        item-title="text"
        item-value="value"
      ></v-select>
    </v-col>
    <v-col>
      <v-select
        v-model="year"
        hide-details="auto"
        label="Jahr"
        :items="years"
      ></v-select>
    </v-col>
  </v-row>
</template>

<script setup lang="ts">
import { ref, watch } from "vue";

const day = ref<number | undefined>();
const month = ref<number | undefined>();
const year = ref<number | undefined>();

const days = ref([...Array(31).keys()].map((i) => i + 1));
const months = [
  { text: "Januar", value: 1 },
  { text: "Februar", value: 2 },
  { text: "März", value: 3 },
  { text: "April", value: 4 },
  { text: "Mai", value: 5 },
  { text: "Juni", value: 6 },
  { text: "Juli", value: 7 },
  { text: "August", value: 8 },
  { text: "September", value: 9 },
  { text: "Oktober", value: 10 },
  { text: "November", value: 11 },
  { text: "Dezember", value: 12 },
];
const years = [...Array(100).keys()].map((i) => new Date().getFullYear() - i);

watch(month, () => {
  const daysInMonth = new Date(
    year.value as number,
    month.value as number,
    0
  ).getDate();
  days.value = [...Array(daysInMonth).keys()].map((i) => i + 1);

  // Reset the selected Tag if it's now out of range
  if (day.value && day.value > daysInMonth) {
    day.value = undefined;
  }
});
</script>
