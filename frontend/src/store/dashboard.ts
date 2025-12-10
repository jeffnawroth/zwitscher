import { defineStore } from 'pinia'
import { ref } from 'vue'
import { DashboardApi } from '@/typescript-axios-generated'
import { showNotification } from './helpers'

export const useDashboardStore = defineStore('dashboard', () => {
  const postsPerDayData = ref<number[]>([])
  const usersGrowthData = ref<number[]>([])
  const activeUsersData = ref<number[]>([])
  const ageDistributionData = ref<number[]>([])
  const genderDistributionData = ref<number[]>([])
  const numberOfPostsToday = ref<number>(0)
  const numberOfActiveUsersToday = ref<number>(0)
  const numberOfUserGrowthToday = ref<number>(0)
  const loadingPostsPerDay = ref(false)
  const loadingUsersGrowth = ref(false)
  const loadingActiveUsers = ref(false)
  const loadingAgeDistribution = ref(false)
  const loadingGenderDistribution = ref(false)
  const loadingActiveUsersToday = ref(false)
  const loadingUsersGrowthToday = ref(false)
  const loadingPostsToday = ref(false)

  /**
   * Returns an array with amount of posts in the last 7 days.
   */
  async function getPostsPerDayData() {
    try {
      loadingPostsPerDay.value = true
      const data = await DashboardApi.prototype.apiDashboardPostsPerDayGet()
      postsPerDayData.value = data.data
    }
    catch {
      showNotification(
        'error',
        'Beim Laden der Anzahl der Posts der letzten 7 Tage ist ein Fehler aufgetreten.',
      )
    }
    finally {
      loadingPostsPerDay.value = false
    }
  }
  /**
   * Returns an array with amount of users created at each month in the last 12 months.
   */
  async function getUsersGrowthData() {
    try {
      loadingUsersGrowth.value = true
      const data = await DashboardApi.prototype.apiDashboardUsersGrowthGet()
      usersGrowthData.value = data.data
    }
    catch {
      showNotification(
        'error',
        'Beim Laden des Nutzerzuwachs der letzten 12 Monate ist ein Fehler aufgetreten.',
      )
    }
    finally {
      loadingUsersGrowth.value = false
    }
  }

  /**
   * Returns an array with amount of active users in the last 12 months.
   */
  async function getActiveUsersData() {
    try {
      loadingActiveUsers.value = true
      const data = await DashboardApi.prototype.apiDashboardActiveUsersGet()
      activeUsersData.value = data.data
    }
    catch {
      showNotification(
        'error',
        'Beim Laden der aktiven Nutzer der letzten 12 Monate ist ein Fehler aufgetreten.',
      )
    }
    finally {
      loadingActiveUsers.value = false
    }
  }

  /**
   * Returns an array with percentage of users ages.
   */
  async function getAgeDistributionData() {
    try {
      loadingAgeDistribution.value = true
      const data
        = await DashboardApi.prototype.apiDashboardAgeDistributionGet()
      ageDistributionData.value = data.data
    }
    catch {
      showNotification(
        'error',
        'Beim Laden der Altersverteilung ist ein Fehler aufgetreten.',
      )
    }
    finally {
      loadingAgeDistribution.value = false
    }
  }

  /**
   * Returns an array with percentages of users gender.
   */
  async function getGenderDistributionData() {
    try {
      loadingGenderDistribution.value = true
      const data
        = await DashboardApi.prototype.apiDashboardGenderDistributionGet()
      genderDistributionData.value = data.data
    }
    catch {
      showNotification(
        'error',
        'Beim Laden der Geschlechterverteilung ist ein Fehler aufgetreten.',
      )
    }
    finally {
      loadingGenderDistribution.value = false
    }
  }
  /**
   * Returns the amount of new posts today.
   */
  async function getNumberOfPostsToday() {
    try {
      loadingPostsToday.value = true
      const data = await DashboardApi.prototype.apiDashboardPostsTodayGet()
      numberOfPostsToday.value = data.data
    }
    catch {
      showNotification(
        'error',
        'Beim Laden der Anzahl der heutigen Posts ist ein Fehler aufgetreten.',
      )
    }
    finally {
      loadingPostsToday.value = false
    }
  }

  /**
   * Returns the amount of new users today.
   */
  async function getNumberOfTodaysUserGrowth() {
    try {
      loadingUsersGrowthToday.value = true
      const data
        = await DashboardApi.prototype.apiDashboardUsersGrowthTodayGet()
      numberOfUserGrowthToday.value = data.data
    }
    catch {
      showNotification(
        'error',
        'Beim Laden des heutigen Nutzerzuwachs ist ein Fehler aufgetreten.',
      )
    }
    finally {
      loadingUsersGrowthToday.value = false
    }
  }

  /**
   * Returns the amount of active users today.
   */
  async function getNumberOfActiveUsersToday() {
    try {
      loadingActiveUsersToday.value = true
      const data
        = await DashboardApi.prototype.apiDashboardActiveUsersTodayGet()
      numberOfActiveUsersToday.value = data.data
    }
    catch {
      showNotification(
        'error',
        'Beim Laden der Anzahl der heute aktiven Nutzer ist ein Fehler aufgetreten.',
      )
    }
    finally {
      loadingActiveUsersToday.value = false
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
    getNumberOfTodaysUserGrowth,
    getNumberOfPostsToday,
    getNumberOfActiveUsersToday,
    loadingActiveUsersToday,
    loadingPostsToday,
    loadingUsersGrowthToday,
    numberOfPostsToday,
    numberOfActiveUsersToday,
    numberOfUserGrowthToday,
  }
})
