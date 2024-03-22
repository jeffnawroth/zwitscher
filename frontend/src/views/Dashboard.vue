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
    <v-col cols="12" sm="4">
      <DashboardCard
        id="posts-today"
        :loading="store.loadingPostsToday"
        title="Posts (heute) "
        @download="downloadPostsToday"
      >
        <span class="text-h2">{{ `+ ${store.numberOfPostsToday}` }}</span>
      </DashboardCard>
    </v-col>
    <v-col cols="12" sm="4">
      <DashboardCard
        id="user-growth-today"
        :loading="store.loadingUsersGrowthToday"
        title="Nutzerzuwachs (heute) "
        @download="downloadUserGrowthToday"
      >
        <span class="text-h2">{{ `+ ${store.numberOfUserGrowthToday}` }}</span>
      </DashboardCard>
    </v-col>
    <v-col cols="12" sm="4">
      <DashboardCard
        id="active-users-today"
        :loading="store.loadingActiveUsersToday"
        title="Aktive Nutzer (heute) "
        @download="downloadActiveUsersToday"
      >
        <span class="text-h2">{{ `${store.numberOfActiveUsersToday}` }}</span>
      </DashboardCard>
    </v-col>
    <v-col cols="12" sm="6">
      <DashboardCard
        id="posts-per-day-chart"
        :loading="store.loadingPostsPerDay"
        :title="`Posts pro Tag (letzte 7 Tage)`"
        @download="downloadPostsPerDay"
      >
      </DashboardCard>
    </v-col>
    <v-col cols="12" sm="6">
      <DashboardCard
        id="users-growth"
        :loading="store.loadingUsersGrowth"
        :title="`Nutzerzuwachs (letzte 12 Monate)`"
        @download="downloadUserGrowth"
      >
      </DashboardCard>
    </v-col>
    <v-col cols="12" sm="6">
      <DashboardCard
        id="active-users-chart"
        :loading="store.loadingActiveUsers"
        :title="`Aktive Nutzer (letzte 12 Monate)`"
        @download="downloadActiveUsers"
      >
      </DashboardCard>
    </v-col>
    <v-col cols="12" sm="6">
      <DashboardCard
        id="age-distribution-chart"
        :loading="store.loadingAgeDistribution"
        title="Altersverteilung"
        @download="downloadAgeDistribution"
      >
      </DashboardCard>
    </v-col>
    <v-col cols="12" sm="6">
      <DashboardCard
        id="gender-distribution-chart"
        :loading="store.loadingGenderDistribution"
        title="Geschlechterverteilung"
        @download="downloadGenderDistribution"
      >
      </DashboardCard>
    </v-col>
  </v-row>
</template>

<script setup lang="ts">
import PageToolbar from "@/components/PageToolbar.vue";
import Chart, { ChartItem } from "chart.js/auto";
import { onMounted, ref } from "vue";
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
  labels: [
    "18-24 Jahre",
    "25-34 Jahre",
    "35-44 Jahre",
    "45-54 Jahre",
    "55+ Jahre",
  ],
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

const images = ref({});

onMounted(async () => {
  await Promise.all([
    fetchPostsPerDay(),
    fetchUserGrowth(),
    fetchActiveUsers(),
    fetchAgeDistribution(),
    fetchGenderDistribution(),
    store.getNumberOfPostsToday(),
    store.getNumberOfActiveUsersToday(),
    store.getNumberOfTodaysUserGrowth(),
  ]);
});

//Helper functions

/**
 * Get the last 7 days
 */
function generateWeekdays() {
  const days = ["So", "Mo", "Di", "Mi", "Do", "Fr", "Sa"];

  const currentDate = new Date();
  const currentDayIndex = currentDate.getDay(); // Aktueller Tag der Woche (0-6)

  const last7Days = [];
  for (let i = 1; i <= 7; i++) {
    const dayIndex = (currentDayIndex - i + 7) % 7; // Index des Tages im Array (rückwärts)
    last7Days.unshift(days[dayIndex]);
  }

  return last7Days;
}

/**
 * Get the last 12 Months
 */
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
  const currentMonth = currentDate.getMonth(); // Aktueller Monat (0-11)

  const last12Months = [];
  for (let i = 1; i <= 12; i++) {
    const monthIndex = (currentMonth - i + 12) % 12; // Index des Monats im Array (rückwärts)
    last12Months.unshift(months[monthIndex]);
  }

  return last12Months;
}

/**
 * Create a excel file
 * @param data
 * @param sheetName
 */
function createExcelFile(data: any, sheetName: string) {
  const workbook = utils.book_new();
  const worksheet = utils.json_to_sheet(data);
  utils.book_append_sheet(workbook, worksheet, sheetName);
  return workbook;
}

//Charts

/**
 * Create a new chart
 */
function createNewChart(
  chartId: string,
  chartType: any,
  chartData: any,
  showLegend: boolean = false,
) {
  const chartElement = document.getElementById(chartId);
  if (chartElement) {
    //@ts-expect-error
    images.value[chartId] = new Chart(chartElement as ChartItem, {
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
    true,
  );
}

//Download functions

/**
 * Download file as excel file
 * @param workbook
 * @param filename
 */
function downloadExcelFile(workbook: WorkBook, filename: string) {
  const excelBuffer = write(workbook, { bookType: "xlsx", type: "array" });
  const blob = new Blob([excelBuffer], {
    type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
  });
  saveAs(blob, filename);
}

function downloadBase64Image(fileName: string, id: string) {
  //@ts-expect-error
  const image = images.value[id].toBase64Image();

  const link = document.createElement("a");
  link.href = image;
  link.download = fileName;
  document.body.appendChild(link);
  link.click();
  document.body.removeChild(link);
}

/**
 * Download number of posts for the last 7 days
 */
function downloadPostsPerDay() {
  const data = postsPerDayData.labels.map((day, index) => ({
    Tag: day,
    "Posts pro Tag": postsPerDayData.datasets[0].data[index],
  }));
  const workbook = createExcelFile(data, "Posts pro Tag");
  downloadExcelFile(workbook, "posts_pro_tag.xlsx");
  downloadBase64Image("posts-pro-tag.png", "posts-per-day-chart");
}

/**
 * Download user growth for the last 12 months
 */
function downloadUserGrowth() {
  const data = usersGrowthData.labels.map((month, index) => ({
    Monat: month,
    Nutzerzuwachs: usersGrowthData.datasets[0].data[index],
  }));
  const workbook = createExcelFile(data, "Nutzerzuwachs");
  downloadExcelFile(workbook, "nutzerzuwachs.xlsx");
  downloadBase64Image("nutzerzuwachs.png", "users-growth");
}

/**
 * Download age distribution
 */
function downloadAgeDistribution() {
  const data = ageDistributionData.labels.map((group, index) => ({
    Altersgruppe: group,
    Anzahl: ageDistributionData.datasets[0].data[index] + "%",
  }));
  const workbook = createExcelFile(data, "Altersverteilung");
  downloadExcelFile(workbook, "altersverteilung.xlsx");
  downloadBase64Image("altersverteilung.png", "age-distribution-chart");
}

/**
 * Download gender distribution
 */ function downloadGenderDistribution() {
  const data = genderDistributionData.labels.map((gender, index) => ({
    Geschlecht: gender,
    Anzahl: genderDistributionData.datasets[0].data[index] + "%",
  }));
  const workbook = createExcelFile(data, "Geschlechterverteilung");
  downloadExcelFile(workbook, "geschlechterverteilung.xlsx");
  downloadBase64Image(
    "geschlechterverteilung.png",
    "gender-distribution-chart",
  );
}

/**
 * Download the number of active users for the last 12 months
 */
function downloadActiveUsers() {
  const data = activeUsersData.labels.map((month, index) => ({
    Monat: month,
    "Aktive Nutzer": activeUsersData.datasets[0].data[index],
  }));
  const workbook = createExcelFile(data, "Aktive Nutzer");
  downloadExcelFile(workbook, "aktive_nutzer.xlsx");
  downloadBase64Image("aktive_nutzer.png", "active-users-chart");
}

/**
 * Download the number of posts from today
 */
function downloadPostsToday() {
  const data = [
    {
      Tag: new Date(),
      Anzahl: store.numberOfPostsToday,
    },
  ];
  const workbook = createExcelFile(data, "Posts (heute)");
  downloadExcelFile(workbook, "posts_heute.xlsx");
}

/**
 * Download the number of user growth from today
 */
function downloadUserGrowthToday() {
  const data = [
    {
      Tag: new Date(),
      Anzahl: store.numberOfUserGrowthToday,
    },
  ];
  const workbook = createExcelFile(data, "Nutzerzuwachs (heute)");
  downloadExcelFile(workbook, "nutzerzuwachs_heute.xlsx");
}

/**
 * Download the number of all active users from today
 */
function downloadActiveUsersToday() {
  const data = [
    {
      Tag: new Date(),
      Anzahl: store.numberOfActiveUsersToday,
    },
  ];
  const workbook = createExcelFile(data, "Aktive Nutzer (heute)");
  downloadExcelFile(workbook, "aktive_nutzer_heute.xlsx");
}

function downloadAllCharts() {
  downloadActiveUsers();
  downloadAgeDistribution();
  downloadGenderDistribution();
  downloadPostsPerDay();
  downloadUserGrowth();
  downloadPostsToday(), downloadActiveUsersToday(), downloadUserGrowthToday();
}
</script>
