import {
  getActiveUsers,
  getAgeDistribution,
  getGenderDistribution,
  getPostsPerDay,
  getUsersGrowth,
} from "@/dummyApi";
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
      const data = await getPostsPerDay();
      postsPerDayData.value = data;
    } catch {
      console.log("error");
    } finally {
      loadingPostsPerDay.value = false;
    }
  }
  async function getUsersGrowthData() {
    try {
      loadingUsersGrowth.value = true;
      const data = await getUsersGrowth();
      usersGrowthData.value = data;
    } catch {
      console.log("error");
    } finally {
      loadingUsersGrowth.value = false;
    }
  }
  async function getActiveUsersData() {
    try {
      loadingActiveUsers.value = true;
      const data = await getActiveUsers();
      activeUsersData.value = data;
    } catch {
      console.log("error");
    } finally {
      loadingActiveUsers.value = false;
    }
  }
  async function getAgeDistributionData() {
    try {
      loadingAgeDistribution.value = true;
      const data = await getAgeDistribution();
      ageDistributionData.value = data;
    } catch {
      console.log("error");
    } finally {
      loadingAgeDistribution.value = false;
    }
  }
  async function getGenderDistributionData() {
    try {
      loadingGenderDistribution.value = true;
      const data = await getGenderDistribution();
      genderDistributionData.value = data;
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
