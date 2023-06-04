import {
  activeUsersData,
  ageDistributionData,
  genderDistributionData,
  postsPerDayData,
  usersGrowthData,
} from "./dummyData";

export const getPostsPerDay = (): Promise<number[]> =>
  new Promise((resolve, reject) => {
    resolve(postsPerDayData);
  });
export const getUsersGrowth = (): Promise<number[]> =>
  new Promise((resolve, reject) => {
    resolve(usersGrowthData);
  });
export const getActiveUsers = (): Promise<number[]> =>
  new Promise((resolve, reject) => {
    resolve(activeUsersData);
  });
export const getAgeDistribution = (): Promise<number[]> =>
  new Promise((resolve, reject) => {
    resolve(ageDistributionData);
  });
export const getGenderDistribution = (): Promise<number[]> =>
  new Promise((resolve, reject) => {
    resolve(genderDistributionData);
  });
