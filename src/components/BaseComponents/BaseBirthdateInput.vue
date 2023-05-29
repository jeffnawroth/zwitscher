<template>
  <v-row>
    <v-col>
      <BaseSelectWithValidation
        name="day"
        label="Tag"
        :items="days"
      ></BaseSelectWithValidation>
    </v-col>
    <v-col>
      <BaseSelectWithValidation
        name="month"
        label="Monat"
        :items="months"
        item-title="text"
        item-value="value"
      ></BaseSelectWithValidation>
    </v-col>
    <v-col>
      <BaseSelectWithValidation
        name="year"
        label="Jahr"
        :items="years"
      ></BaseSelectWithValidation>
    </v-col>
  </v-row>
</template>

<script setup lang="ts">
import { ref, toRef, watch } from "vue";
import BaseSelectWithValidation from "./BaseSelectWithValidation.vue";
import { useField } from "vee-validate";

const props = defineProps({
  nameDay: {
    type: String,
    required: true,
  },
  nameMonth: {
    type: String,
    required: true,
  },
  nameYear: {
    type: String,
    required: true,
  },
});

const { value: day } = useField<number | undefined>(
  toRef(props, "nameDay"),
  undefined
);
const { value: month } = useField<number | undefined>(
  toRef(props, "nameMonth"),
  undefined
);
const { value: year } = useField<number | undefined>(
  toRef(props, "nameYear"),
  undefined
);

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
  const daysInMonth = new Date(year.value ?? 0, month.value ?? 0, 0).getDate();

  days.value = [...Array(daysInMonth).keys()].map((i) => i + 1);

  // Reset the selected Tag if it's now out of range
  if (day.value && day.value > daysInMonth) {
    day.value = undefined;
  }
});
</script>
