import { AuthUser, Post, User } from "@/interfaces";

async function getAvatar(avatar: string): Promise<File> {
  const response = await fetch(avatar);
  const blob = await response.blob();
  return new File([blob], "avatar.jpg", { type: "image/jpeg" });
}

export const userData: AuthUser = {
  id: 0,
  token:
    "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c",
  name: "Admin Nimda",
  role: "Admin",
  username: "ANimda",
  email: "admin@nimda.de",
  password: "Admin1!",
  avatar: await getAvatar(
    "https://cdn.pixabay.com/photo/2020/06/24/19/12/cabbage-5337431_1280.jpg"
  ),
  follower: [1, 2],
  following: [1, 2],
  liked: [],
  disliked: [],
  bio: "Lorem ipsum dolor, sit amet consectetur adipisicing elit. Facere ex illum ad? Tenetur dolore accusantium laborum amet doloribus dignissimos reiciendis eveniet, exercitationem dicta perferendis, ullam cupiditate. Quidem sed non nulla!",
  createdAt: new Date(),
  birthdate: new Date().toISOString().slice(0, 10),
  gender: "männlich",
  interests: ["Schach", "Bücher", "Fußball"],
  refreshToken:
    "jfklasjfklajslfjklasjdfjasdkljfklsdjfkljsdlafjlkasdjfkljasdlfjasdklfjasdljflsdj",
  locked: false,
};

export const users: User[] = [
  {
    id: 0,
    name: "Admin Nimda",
    role: "Admin",
    username: "ANimda",
    email: "admin@nimda.de",
    avatar: await getAvatar(
      "https://cdn.pixabay.com/photo/2020/06/24/19/12/cabbage-5337431_1280.jpg"
    ),
    follower: [1, 2],
    following: [1, 2],
    liked: [],
    disliked: [],
    bio: "Lorem ipsum dolor, sit amet consectetur adipisicing elit. Facere ex illum ad? Tenetur dolore accusantium laborum amet doloribus dignissimos reiciendis eveniet, exercitationem dicta perferendis, ullam cupiditate. Quidem sed non nulla!",
    createdAt: new Date(),
    gender: "männlich",
    birthdate: new Date().toISOString().slice(0, 10),
    interests: ["Schach", "Bücher", "Fußball"],
    locked: false,
  },
  {
    id: 1,
    username: "JDoe",
    name: "John Doe",
    email: "john.doe@example.com",
    role: "Admin",
    follower: [2, 0],
    following: [2, 0],
    liked: [],
    disliked: [],
    avatar: await getAvatar(
      "https://cdn.pixabay.com/photo/2023/05/23/15/26/bengal-cat-8012976_960_720.jpg"
    ),
    bio: "Hallo das ist meine Bio.",
    createdAt: new Date(),
    locked: false,
  },
  {
    id: 2,
    username: "JaneDoe",
    name: "Jane Doe",
    email: "jane.doe@example.com",
    role: "Moderator",
    follower: [1, 0],
    following: [1, 0],
    liked: [],
    disliked: [],
    avatar: await getAvatar(
      "https://avatars0.githubusercontent.com/u/9064066?v=4&s=460"
    ),
    bio: "Hallo das ist meine Bio.",
    createdAt: new Date(),
    birthdate: new Date().toISOString().slice(0, 10),
    locked: false,
  },
];

export const allPosts: Post[] = [
  {
    id: 1,
    userId: 0,
    name: "Admin Nimda",
    username: "ANimda",
    avatar: await getAvatar(
      "https://cdn.pixabay.com/photo/2020/06/24/19/12/cabbage-5337431_1280.jpg"
    ),
    upvotes: 20,
    downvotes: 5,
    text: "Just finished reading a great book about astrophysics. Highly recommend it!",
    date: new Date("2023-05-1"),
    comments: [],
    files: [],
  },
  {
    id: 2,
    userId: 0,
    name: "Admin Nimda",
    username: "ANimda",
    avatar: await getAvatar(
      "https://cdn.pixabay.com/photo/2020/06/24/19/12/cabbage-5337431_1280.jpg"
    ),
    upvotes: 20,
    downvotes: 5,
    text: "Just finished reading a great book about astrophysics. Highly recommend it!",
    date: new Date("2023-05-02T18:56:00"),
    comments: [],
    files: [],
  },
  {
    id: 3,
    userId: 2,
    name: "Admin Nimda",
    username: "JaneDoe",
    avatar: await getAvatar(
      "https://avatars0.githubusercontent.com/u/9064066?v=4&s=460"
    ),
    upvotes: 15,
    downvotes: 10,
    text: "I'm planning a road trip across the US this summer. Any recommendations?",
    date: new Date("2022-04-11"),
    comments: [],
    files: [],
  },
  {
    id: 4,
    userId: 1,
    name: "John Doe",
    username: "JDoe",
    avatar: await getAvatar(
      "https://cdn.pixabay.com/photo/2023/05/23/15/26/bengal-cat-8012976_960_720.jpg"
    ),
    upvotes: 20,
    downvotes: 5,
    text: "Just finished reading a great book about astrophysics. Highly recommend it!",
    date: new Date("2023-04-27"),
    comments: [],
    files: [],
  },
];

export const userPosts: Post[] = [
  {
    id: 1,
    userId: 0,
    name: "Admin Nimda",
    username: "ANimda",
    avatar: await getAvatar(
      "https://cdn.pixabay.com/photo/2020/06/24/19/12/cabbage-5337431_1280.jpg"
    ),
    upvotes: 20,
    downvotes: 5,
    text: "Just finished reading a great book about astrophysics. Highly recommend it!",
    date: new Date("2023-05-1"),
    comments: [],
    files: [],
  },
  {
    id: 2,
    userId: 0,
    name: "Admin Nimda",

    username: "ANimda",
    avatar: await getAvatar(
      "https://cdn.pixabay.com/photo/2020/06/24/19/12/cabbage-5337431_1280.jpg"
    ),
    upvotes: 20,
    downvotes: 5,
    text: "Just finished reading a great book about astrophysics. Highly recommend it!",
    date: new Date("2023-05-02T18:56:00"),
    comments: [],
    files: [],
  },
];

export const followedUsersPosts: Post[] = [
  {
    id: 3,
    userId: 2,
    name: "Jane Doe",
    username: "JaneDoe",
    avatar: await getAvatar(
      "https://avatars0.githubusercontent.com/u/9064066?v=4&s=460"
    ),
    upvotes: 15,
    downvotes: 10,
    text: "I'm planning a road trip across the US this summer. Any recommendations?",
    date: new Date("2022-04-11"),
    comments: [],
    files: [],
  },
  {
    id: 4,
    userId: 1,
    name: "John Doe",
    username: "JDoe",
    avatar: await getAvatar(
      "https://cdn.pixabay.com/photo/2023/05/23/15/26/bengal-cat-8012976_960_720.jpg"
    ),
    upvotes: 20,
    downvotes: 5,
    text: "Just finished reading a great book about astrophysics. Highly recommend it!",
    date: new Date("2023-04-27"),
    comments: [],
    files: [],
  },
];
