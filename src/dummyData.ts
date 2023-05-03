import { AuthUser, Post, User } from "@/interfaces";

export const userData: AuthUser = {
  firstName: "Admin",
  lastName: "Nimda",
  token:
    "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c",
  id: 0,
  role: "Admin",
  username: "ANimda",
  email: "admin@nimda.de",
  password: "Admin1!",
  avatar:
    "https://cdn.pixabay.com/photo/2020/06/24/19/12/cabbage-5337431_1280.jpg",
  follower: 32,
  following: 14,
  liked: [],
  disliked: [],
};

export const users: User[] = [
  {
    firstName: "Admin",
    lastName: "Nimda",
    id: 0,
    role: "Admin",
    username: "ANimda",
    email: "admin@nimda.de",
    password: "Admin1!",
    avatar:
      "https://cdn.pixabay.com/photo/2020/06/24/19/12/cabbage-5337431_1280.jpg",
    follower: 32,
    following: 14,
  },
  {
    id: 1,
    username: "JDoe",
    firstName: "John",
    lastName: "Doe",
    email: "john.doe@example.com",
    role: "Admin",
    password: "789das789dfas789dfs",
    follower: 31,
    following: 14,
  },
  {
    id: 2,
    username: "JaneDoe",
    firstName: "Jane",
    lastName: "Doe",
    email: "jane.doe@example.com",
    role: "Moderator",
    password: "789das789dfas789dfs",
    follower: 31,
    following: 14,
  },
];

export const allPosts: Post[] = [
  {
    id: 1,
    userId: 0,
    firstName: "Admin",
    lastName: "Nimda",
    username: "ANimda",
    avatar:
      "https://cdn.pixabay.com/photo/2020/06/24/19/12/cabbage-5337431_1280.jpg",
    upvotes: 20,
    downvotes: 5,
    text: "Just finished reading a great book about astrophysics. Highly recommend it!",
    date: new Date("2023-05-1"),
  },
  {
    id: 2,
    userId: 0,
    firstName: "Admin",
    lastName: "Nimda",
    username: "ANimda",
    avatar:
      "https://cdn.pixabay.com/photo/2020/06/24/19/12/cabbage-5337431_1280.jpg",
    upvotes: 20,
    downvotes: 5,
    text: "Just finished reading a great book about astrophysics. Highly recommend it!",
    date: new Date("2023-05-02T18:56:00"),
  },
  {
    id: 3,
    userId: 2,
    firstName: "Jane",
    lastName: "Doe",
    username: "JaneDoe",
    avatar: "https://avatars0.githubusercontent.com/u/9064066?v=4&s=460",
    upvotes: 15,
    downvotes: 10,
    text: "I'm planning a road trip across the US this summer. Any recommendations?",
    date: new Date("2022-04-11"),
  },
  {
    id: 4,
    userId: 1,
    firstName: "John",
    lastName: "Doe",
    username: "JDoe",
    avatar: "https://cdn.vuetifyjs.com/images/john.jpg",
    upvotes: 20,
    downvotes: 5,
    text: "Just finished reading a great book about astrophysics. Highly recommend it!",
    date: new Date("2023-04-27"),
  },
];

export const userPosts: Post[] = [
  {
    id: 1,
    userId: 0,
    firstName: "Admin",
    lastName: "Nimda",
    username: "ANimda",
    avatar:
      "https://cdn.pixabay.com/photo/2020/06/24/19/12/cabbage-5337431_1280.jpg",
    upvotes: 20,
    downvotes: 5,
    text: "Just finished reading a great book about astrophysics. Highly recommend it!",
    date: new Date("2023-05-1"),
  },
  {
    id: 2,
    userId: 0,
    firstName: "Admin",
    lastName: "Nimda",
    username: "ANimda",
    avatar:
      "https://cdn.pixabay.com/photo/2020/06/24/19/12/cabbage-5337431_1280.jpg",
    upvotes: 20,
    downvotes: 5,
    text: "Just finished reading a great book about astrophysics. Highly recommend it!",
    date: new Date("2023-05-02T18:56:00"),
  },
];
