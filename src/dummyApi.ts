import {
  activeUsersData,
  ageDistributionData,
  allPosts,
  genderDistributionData,
  postsPerDayData,
  usersGrowthData,
  allUsers,
} from "./dummyData";
import { PostAdd, Post, User, UserAdd } from "./interfaces";
import { v4 as uuidv4 } from "uuid";

//Posts

let posts = allPosts;
let users = allUsers;

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
    upVotes: 0,
    downVotes: 0,
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

export const getAllUsers = (): Promise<User[]> =>
  new Promise((resolve) => resolve(users));

export const getUserById = (id: string): Promise<User> =>
  new Promise((resolve, reject) => {
    const user = users.find((user) => user.id === id);
    if (user) resolve(user);
    else reject();
  });

export const fetchUserByUsername = (username: string): Promise<User> =>
  new Promise((resolve, reject) => {
    const user = users.find((user) => user.username === username);
    if (user) resolve(user);
    else reject();
  });

export const removeUser = (id: string) =>
  new Promise((resolve) => {
    users = users.filter((user) => user.id !== id);
    resolve("Der Nutzer wurde erfolgreich gelöscht");
  });

export const modifyUser = (user: User) =>
  new Promise((resolve) => {
    const index = users.findIndex((oldUser) => oldUser.id === user.id);
    if (index > -1) {
      users.splice(index, 1, user);
      resolve("Update erfolgreich");
    }
  });

export const createNewUser = (userAdd: UserAdd): Promise<User> =>
  new Promise((resolve) => {
    const user = {
      ...userAdd,
      id: uuidv4(),
      followers: [],
      following: [],
      createdAt: new Date().toUTCString(),
      locked: false,
    };
    users.push(user);
    resolve(user);
  });

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
