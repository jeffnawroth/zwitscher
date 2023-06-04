import {
  activeUsersData,
  ageDistributionData,
  allPosts,
  genderDistributionData,
  postsPerDayData,
  usersGrowthData,
  users,
} from "./dummyData";
import { PostAdd, Post } from "./interfaces";
import { v4 as uuidv4 } from "uuid";

//Posts

let posts = allPosts;

export const getAllPublicPosts = (): Promise<Post[]> =>
  new Promise((resolve) => {
    resolve(posts.slice());
  });
export const getAllPostsFromUser = (id: string): Promise<Post[]> =>
  new Promise((resolve) => {
    resolve(posts.filter((post) => post.userId === id));
  });

export const getPostsFromFollowedUsers = (ids: string[]): Promise<Post[]> => {
  const followedUsersPosts = posts.filter((post) => ids.includes(post.userId));
  return new Promise((resolve) => {
    resolve(followedUsersPosts);
  });
};

export const getSinglePost = (id: string): Promise<Post> => {
  const post = posts.find((post) => post.id === id);
  return new Promise((resolve, reject) => {
    if (post) resolve(post);
    else reject();
  });
};

export const removePost = (id: string) => {
  return new Promise((resolve) => {
    posts = posts.filter((post) => post.id !== id);
    resolve("Erfolgreich gelöscht");
  });
};

export const createNewPost = (postAdd: PostAdd): Promise<Post> => {
  const user = users.find((user) => user.id === postAdd.userId);
  const post: Post = {
    id: uuidv4(),
    userId: postAdd.userId,
    upvotes: 0,
    downvotes: 0,
    name: user?.name!,
    username: user?.username!,
    date: new Date().toUTCString(),
    avatar: user?.avatar,
    comments: [],
    files: postAdd.files,
    text: postAdd.text,
  };
  posts.push(post);
  return new Promise((resolve) => {
    resolve(post);
  });
};

export const modifyPost = (postUpdate: Post) => {
  const index = posts.findIndex((post) => post.id === postUpdate.id);
  if (index > -1) posts.splice(index, 1, postUpdate);
  return new Promise((resolve) => resolve("Update erfolgreich"));
};

//Users

//Dashboard
export const getPostsPerDay = (): Promise<number[]> =>
  new Promise((resolve) => {
    resolve(postsPerDayData);
  });
export const getUsersGrowth = (): Promise<number[]> =>
  new Promise((resolve) => {
    resolve(usersGrowthData);
  });
export const getActiveUsers = (): Promise<number[]> =>
  new Promise((resolve) => {
    resolve(activeUsersData);
  });
export const getAgeDistribution = (): Promise<number[]> =>
  new Promise((resolve) => {
    resolve(ageDistributionData);
  });
export const getGenderDistribution = (): Promise<number[]> =>
  new Promise((resolve) => {
    resolve(genderDistributionData);
  });
