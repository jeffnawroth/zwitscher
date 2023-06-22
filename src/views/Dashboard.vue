<template>
  <PageToolbar icon="mdi-view-dashboard" title="Dashboard">
    <v-btn
      prepend-icon="mdi-download-multiple"
      variant="tonal"
      @click="downloadAllCharts"
      >Alle downloaden</v-btn
    >
  </PageToolbar>
  <v-row class="pa-5">
    <v-col cols="12" sm="6">
      <DashboardCard
        id="posts-per-day-chart"
        :loading="store.loadingPostsPerDay"
        :title="`Posts pro Tag (KW ${getWeekNumber})`"
        @download="downloadPostsPerDay"
      >
      </DashboardCard>
    </v-col>
    <v-col cols="12" sm="6">
      <DashboardCard
        id="users-growth"
        :loading="store.loadingUsersGrowth"
        :title="`Nutzerzuwachs (${currentYear})`"
        @download="downloadUserGrowth"
      >
      </DashboardCard>
    </v-col>
    <v-col cols="12" sm="6">
      <DashboardCard
        id="active-users-chart"
        :loading="store.loadingActiveUsers"
        :title="`Aktive Nutzer (${currentYear})`"
        @download="downloadActiveUsers"
      >
      </DashboardCard>
    </v-col>
    <v-col cols="12" sm="6">
      <DashboardCard
        id="age-distribution-chart"
        :loading="store.loadingAgeDistribution"
        title="Altersverteilung der Nutzer"
        @download="downloadAgeDistribution"
      >
      </DashboardCard>
    </v-col>
    <v-col cols="12" sm="6">
      <DashboardCard
        id="gender-distribution-chart"
        :loading="store.loadingGenderDistribution"
        title="Geschlechterverteilung der Nutzer"
        @download="downloadGenderDistribution"
      >
      </DashboardCard>
    </v-col>
  </v-row>
</template>

<script setup lang="ts">
import PageToolbar from "@/components/PageToolbar.vue";
import Chart, { ChartItem } from "chart.js/auto";
import { computed } from "vue";
import { onMounted } from "vue";
import DashboardCard from "@/components/Dashboard/DashboardCard.vue";
import { utils, write, WorkBook } from "xlsx";
import { saveAs } from "file-saver";
import { useDashboardStore } from "@/store/dashboard";

const store = useDashboardStore();

const genderDistributionData = {
  labels: ["Männlich", "Weiblich", "Divers"],
  datasets: [
    {
      data: [] as number[],
      backgroundColor: [
        "rgba(54, 162, 235, 0.7)",
        "rgba(255, 99, 132, 0.7)",
        "rgba(255, 205, 86, 0.7)",
      ],
      borderColor: [
        "rgba(54, 162, 235, 1)",
        "rgba(255, 99, 132, 1)",
        "rgba(255, 205, 86, 1)",
      ],
      borderWidth: 1,
    },
  ],
};

const ageDistributionData = {
  labels: ["18-24 Jahre", "25-34 Jahre", "35-44 Jahre", "45-54 Jahre", "55+"],
  datasets: [
    {
      data: [] as number[],
      backgroundColor: [
        "rgba(255, 99, 132, 0.7)",
        "rgba(54, 162, 235, 0.7)",
        "rgba(255, 205, 86, 0.7)",
        "rgba(75, 192, 192, 0.7)",
        "rgba(153, 102, 255, 0.7)",
      ],
      borderColor: [
        "rgba(255, 99, 132, 1)",
        "rgba(54, 162, 235, 1)",
        "rgba(255, 205, 86, 1)",
        "rgba(75, 192, 192, 1)",
        "rgba(153, 102, 255, 1)",
      ],
      borderWidth: 1,
    },
  ],
};

const activeUsersData = {
  labels: generateMonthLabels(),
  datasets: [
    {
      label: "Aktive Nutzer",
      data: [] as number[],
      backgroundColor: "rgba(75, 192, 192, 0.7)",
      borderColor: "rgba(75, 192, 192, 1)",
      borderWidth: 1,
    },
  ],
};

const postsPerDayData = {
  labels: generateWeekdays(),
  datasets: [
    {
      label: "Posts",
      data: [] as number[],
      backgroundColor: "rgba(0, 123, 255, 0.5)",
      borderColor: "rgba(0, 123, 255, 1)",
      borderWidth: 1,
    },
  ],
};

const usersGrowthData = {
  labels: generateMonthLabels(),
  datasets: [
    {
      data: [] as number[],
      backgroundColor: "rgba(0, 123, 255, 0.5)",
      borderColor: "rgba(0, 123, 255, 1)",
      borderWidth: 1,
    },
  ],
};

const currentYear = computed(() => {
  return new Date().getFullYear();
});

function generateWeekdays() {
  const weekdays = ["Mo", "Di", "Mi", "Do", "Fr", "Sa", "So"];
  return weekdays;
}

const getWeekNumber = computed(() => {
  const date = new Date();
  const d = new Date(
    Date.UTC(date.getFullYear(), date.getMonth(), date.getDate())
  );
  const dayNum = d.getUTCDay() || 7;
  d.setUTCDate(d.getUTCDate() + 4 - dayNum);
  const yearStart = new Date(Date.UTC(d.getUTCFullYear(), 0, 1));
  //@ts-expect-error
  return Math.ceil(((d - yearStart) / 86400000 + 1) / 7);
});

onMounted(async () => {
  await Promise.all([
    fetchPostsPerDay(),
    fetchUserGrowth(),
    fetchActiveUsers(),
    fetchAgeDistribution(),
    fetchGenderDistribution(),
  ]);
});

async function fetchPostsPerDay() {
  await store.getPostsPerDayData();
  postsPerDayData.datasets[0].data = store.postsPerDayData;
  createNewChart("posts-per-day-chart", "bar", postsPerDayData);
}
async function fetchUserGrowth() {
  await store.getUsersGrowthData();
  usersGrowthData.datasets[0].data = store.usersGrowthData;
  createNewChart("users-growth", "line", usersGrowthData);
}
async function fetchActiveUsers() {
  await store.getActiveUsersData();
  activeUsersData.datasets[0].data = store.activeUsersData;
  createNewChart("active-users-chart", "line", activeUsersData);
}
async function fetchAgeDistribution() {
  await store.getAgeDistributionData();
  ageDistributionData.datasets[0].data = store.ageDistributionData;
  createNewChart("age-distribution-chart", "bar", ageDistributionData);
}
async function fetchGenderDistribution() {
  await store.getGenderDistributionData();
  genderDistributionData.datasets[0].data = store.genderDistributionData;
  createNewChart(
    "gender-distribution-chart",
    "doughnut",
    genderDistributionData,
    true
  );
}

function generateMonthLabels() {
  const months = [
    "Jan",
    "Feb",
    "März",
    "Apr",
    "Mai",
    "Jun",
    "Jul",
    "Aug",
    "Sep",
    "Okt",
    "Nov",
    "Dez",
  ];
  const currentDate = new Date();
  const currentMonth = currentDate.getMonth();
  const labels = months.slice(0, currentMonth + 1);
  return labels;
}

function createNewChart(
  chartId: string,
  chartType: any,
  chartData: any,
  showLegend: boolean = false
) {
  const chartElement = document.getElementById(chartId);
  if (chartElement) {
    new Chart(chartElement as ChartItem, {
      type: chartType,
      data: chartData,
      options: {
        responsive: true,
        maintainAspectRatio: false,
        // scales: {
        //   y: {
        //     beginAtZero: true,
        //   },
        // },
        plugins: {
          legend: {
            display: showLegend,
          },
        },
      },
    });
  }
}

function createExcelFile(data: any, sheetName: string) {
  const workbook = utils.book_new();
  const worksheet = utils.json_to_sheet(data);
  utils.book_append_sheet(workbook, worksheet, sheetName);
  return workbook;
}

// Funktion zum Herunterladen der Excel-Datei
function downloadExcelFile(workbook: WorkBook, filename: string) {
  const excelBuffer = write(workbook, { bookType: "xlsx", type: "array" });
  const blob = new Blob([excelBuffer], {
    type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
  });
  saveAs(blob, filename);
}

// Funktion zum Herunterladen der Posts pro Tag als Excel-Datei
function downloadPostsPerDay() {
  const data = postsPerDayData.labels.map((day, index) => ({
    Tag: day,
    "Posts pro Tag": postsPerDayData.datasets[0].data[index],
  }));
  const workbook = createExcelFile(data, "Posts pro Tag");
  downloadExcelFile(workbook, "posts_pro_tag.xlsx");
}

// Funktion zum Herunterladen des Nutzerzuwachses als Excel-Datei
function downloadUserGrowth() {
  const data = usersGrowthData.labels.map((month, index) => ({
    Monat: month,
    Nutzerzuwachs: usersGrowthData.datasets[0].data[index],
  }));
  const workbook = createExcelFile(data, "Nutzerzuwachs");
  downloadExcelFile(workbook, "nutzerzuwachs.xlsx");
}

// Funktion zum Herunterladen der Altersverteilung als Excel-Datei
function downloadAgeDistribution() {
  const data = ageDistributionData.labels.map((group, index) => ({
    Altersgruppe: group,
    Anzahl: ageDistributionData.datasets[0].data[index],
  }));
  const workbook = createExcelFile(data, "Altersverteilung");
  downloadExcelFile(workbook, "altersverteilung.xlsx");
}

// Funktion zum Herunterladen der Geschlechterverteilung als Excel-Datei
function downloadGenderDistribution() {
  const data = genderDistributionData.labels.map((gender, index) => ({
    Geschlecht: gender,
    Anzahl: genderDistributionData.datasets[0].data[index],
  }));
  const workbook = createExcelFile(data, "Geschlechterverteilung");
  downloadExcelFile(workbook, "geschlechterverteilung.xlsx");
}

// Funktion zum Herunterladen der aktiven Nutzer als Excel-Datei
function downloadActiveUsers() {
  const data = activeUsersData.labels.map((month, index) => ({
    Monat: month,
    "Aktive Nutzer": activeUsersData.datasets[0].data[index],
  }));
  const workbook = createExcelFile(data, "Aktive Nutzer");
  downloadExcelFile(workbook, "aktive_nutzer.xlsx");
}

function downloadAllCharts() {
  downloadActiveUsers();
  downloadAgeDistribution();
  downloadGenderDistribution();
  downloadPostsPerDay();
  downloadUserGrowth();
}
</script>
