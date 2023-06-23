import { DashboardApi } from "@/typescript-axios-generated";
import { defineStore } from "pinia";
import { ref } from "vue";

export const useDashboardStore = defineStore("dashboard", () => {
  const postsPerDayData = ref<number[]>([]);
  const usersGrowthData = ref<number[]>([]);
  const activeUsersData = ref<number[]>([]);
  const ageDistributionData = ref<number[]>([]);
  const genderDistributionData = ref<number[]>([]);
  const loadingPostsPerDay = ref(false);
  const loadingUsersGrowth = ref(false);
  const loadingActiveUsers = ref(false);
  const loadingAgeDistribution = ref(false);
  const loadingGenderDistribution = ref(false);

  async function getPostsPerDayData() {
    try {
      loadingPostsPerDay.value = true;
      const data = await DashboardApi.prototype.apiDashboardPostsPerDayGet();
      postsPerDayData.value = data.data;
    } catch {
      console.log("error");
    } finally {
      loadingPostsPerDay.value = false;
    }
  }
  async function getUsersGrowthData() {
    try {
      loadingUsersGrowth.value = true;
      const data = await DashboardApi.prototype.apiDashboardUsersGrowthGet();
      usersGrowthData.value = data.data;
    } catch {
      console.log("error");
    } finally {
      loadingUsersGrowth.value = false;
    }
  }
  async function getActiveUsersData() {
    try {
      loadingActiveUsers.value = true;
      const data = await DashboardApi.prototype.apiDashboardActiveUsersGet();
      activeUsersData.value = data.data;
    } catch {
      console.log("error");
    } finally {
      loadingActiveUsers.value = false;
    }
  }
  async function getAgeDistributionData() {
    try {
      loadingAgeDistribution.value = true;
      const data = await DashboardApi.prototype.apiDashboardAgeDistributionGet();
      ageDistributionData.value = data.data;
    } catch {
      console.log("error");
    } finally {
      loadingAgeDistribution.value = false;
    }
  }
  async function getGenderDistributionData() {
    try {
      loadingGenderDistribution.value = true;
      const data = await DashboardApi.prototype.apiDashboardGenderDistributionGet();
      genderDistributionData.value = data.data;
    } catch {
      console.log("error");
    } finally {
      loadingGenderDistribution.value = false;
    }
  }

  return {
    postsPerDayData,
    usersGrowthData,
    activeUsersData,
    ageDistributionData,
    genderDistributionData,
    getPostsPerDayData,
    getUsersGrowthData,
    getActiveUsersData,
    getAgeDistributionData,
    getGenderDistributionData,
    loadingPostsPerDay,
    loadingActiveUsers,
    loadingAgeDistribution,
    loadingUsersGrowth,
    loadingGenderDistribution,
  };
});
