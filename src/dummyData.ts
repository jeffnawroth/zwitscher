import { AuthUser, Post, User, UserLight } from "@/interfaces";
import { Gender, Role } from "./typescript-axios-generated";

async function getAvatar(avatar: string): Promise<File> {
  const response = await fetch(avatar);
  const blob = await response.blob();
  return new File([blob], "avatar.jpg", { type: "image/jpeg" });
}

const adminId = "b586e624-a6c4-44af-abd3-1180671f7691";
const johnId = "b586e624-a6c4-44af-abd3-1180671f7692";
const janeId = "b586e624-a6c4-44af-abd3-1180671f7693";

export const followedUsers: UserLight[] = [
  {
    id: adminId,
    username: "ANimda",
    name: "Admin Nimda",
    avatar: undefined,
  },
  {
    id: janeId,
    username: "jDoe",
    name: "Jane Doe",
    avatar: undefined,
  },
  {
    id: johnId,
    username: "jDoe",
    name: "John Doe",
    avatar: undefined,
  },
];

export const userData: AuthUser = {
  id: adminId,
  token:
    "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c",
  name: "Admin Nimda",
  role: Role.NUMBER_0,
  username: "ANimda",
  email: "admin@nimda.de",
  password: "Admin1!",
  avatar: await getAvatar(
    "https://cdn.pixabay.com/photo/2020/06/24/19/12/cabbage-5337431_1280.jpg"
  ),
  followers: [johnId, janeId],
  following: [johnId, janeId],
  likedPosts: [],
  dislikedPosts: [],
  bio: "Lorem ipsum dolor, sit amet consectetur adipisicing elit. Facere ex illum ad? Tenetur dolore accusantium laborum amet doloribus dignissimos reiciendis eveniet, exercitationem dicta perferendis, ullam cupiditate. Quidem sed non nulla!",
  createdAt: new Date().toUTCString(),
  birthDate: new Date().toISOString().slice(0, 10),
  gender: Gender.NUMBER_0,
  interests: ["Schach", "Bücher", "Fußball"],
  refreshToken:
    "jfklasjfklajslfjklasjdfjasdkljfklsdjfkljsdlafjlkasdjfkljasdlfjasdklfjasdljflsdj",
  locked: false,
};

// export const allUsers: User[] = [
//   {
//     id: "4cfe63a1-19dc-4703-9551-a90f09ace4f8",
//     name: "Admin",
//     role: Role.NUMBER_0,
//     username: "Admin",
//     email: "admin@zwitscher.de",
//     followers: [],
//     following: [],
//     likedPosts: [],
//     dislikedPosts: [],
//     createdAt: new Date("2023-06-01").toUTCString(),
//     locked: false,
//   },
//   {
//     id: adminId,
//     name: "Admin Nimda",
//     role: Role.NUMBER_0,
//     username: "ANimda",
//     email: "admin@nimda.de",
//     avatar: await getAvatar(
//       "https://cdn.pixabay.com/photo/2020/06/24/19/12/cabbage-5337431_1280.jpg"
//     ),
//     followers: [johnId, janeId],
//     following: [johnId, janeId],
//     likedPosts: [],
//     dislikedPosts: [],
//     bio: "Lorem ipsum dolor, sit amet consectetur adipisicing elit. Facere ex illum ad? Tenetur dolore accusantium laborum amet doloribus dignissimos reiciendis eveniet, exercitationem dicta perferendis, ullam cupiditate. Quidem sed non nulla!",
//     createdAt: new Date().toUTCString(),
//     birthDate: new Date().toISOString().slice(0, 10),
//     gender: Gender.NUMBER_0,
//     interests: ["Schach", "Bücher", "Fußball"],
//     locked: false,
//   },
//   {
//     id: johnId,
//     username: "JDoe",
//     name: "John Doe",
//     email: "john.doe@example.com",
//     role: Role.NUMBER_1,
//     followers: [janeId, adminId],
//     following: [janeId, adminId],
//     likedPosts: [],
//     dislikedPosts: [],
//     avatar: await getAvatar(
//       "https://cdn.pixabay.com/photo/2023/05/23/15/26/bengal-cat-8012976_960_720.jpg"
//     ),
//     bio: "Hallo das ist meine Bio.",
//     createdAt: new Date().toUTCString(),
//     locked: false,
//   },
//   {
//     id: janeId,
//     username: "JaneDoe",
//     name: "Jane Doe",
//     email: "jane.doe@example.com",
//     role: Role.NUMBER_2,
//     followers: [johnId, adminId],
//     following: [johnId, adminId],
//     likedPosts: [],
//     dislikedPosts: [],
//     avatar: await getAvatar(
//       "https://avatars0.githubusercontent.com/u/9064066?v=4&s=460"
//     ),
//     bio: "Hallo das ist meine Bio.",
//     createdAt: new Date().toUTCString(),
//     birthDate: new Date().toISOString().slice(0, 10),
//     locked: false,
//   },
// ];

const adminPost1Id = "614c44f0-f28e-41d7-a4a2-3e6e79157d1b";
const adminPost2Id = "614c44f0-f28e-41d7-a4a2-3e6e79157d2b";
const janePost3Id = "614c44f0-f28e-41d7-a4a2-3e6e79157d3b";
const johnPost4Id = "614c44f0-f28e-41d7-a4a2-3e6e79157d4b";

export const allPosts: Post[] = [
  // {
  //   id: adminPost1Id,
  //   userId: adminId,
  //   name: "Admin Nimda",
  //   username: "ANimda",
  //   avatar: await getAvatar(
  //     "https://cdn.pixabay.com/photo/2020/06/24/19/12/cabbage-5337431_1280.jpg"
  //   ),
  //   upVotes: 20,
  //   downVotes: 5,
  //   text: "Just finished reading a great book about astrophysics. Highly recommend it!",
  //   date: new Date("2023-05-01").toUTCString(),
  //   comments: [],
  //   files: [],
  //   role: Role.NUMBER_0,
  // },
  // {
  //   id: adminPost2Id,
  //   userId: adminId,
  //   name: "Admin Nimda",
  //   username: "ANimda",
  //   avatar: await getAvatar(
  //     "https://cdn.pixabay.com/photo/2020/06/24/19/12/cabbage-5337431_1280.jpg"
  //   ),
  //   upVotes: 20,
  //   downVotes: 5,
  //   text: "Just finished reading a great book about astrophysics. Highly recommend it!",
  //   date: new Date("2023-05-02T18:56:00").toUTCString(),
  //   comments: [],
  //   files: [],
  //   role: Role.NUMBER_0,
  // },
  // {
  //   id: janePost3Id,
  //   userId: janeId,
  //   name: "Jane Doe",
  //   username: "JaneDoe",
  //   avatar: await getAvatar(
  //     "https://avatars0.githubusercontent.com/u/9064066?v=4&s=460"
  //   ),
  //   upVotes: 15,
  //   downVotes: 10,
  //   text: "I'm planning a road trip across the US this summer. Any recommendations?",
  //   date: new Date("2022-04-11").toUTCString(),
  //   comments: [],
  //   files: [],
  //   role: Role.NUMBER_1,
  // },
  // {
  //   id: johnPost4Id,
  //   userId: johnId,
  //   name: "John Doe",
  //   username: "JDoe",
  //   avatar: await getAvatar(
  //     "https://cdn.pixabay.com/photo/2023/05/23/15/26/bengal-cat-8012976_960_720.jpg"
  //   ),
  //   upVotes: 20,
  //   downVotes: 5,
  //   text: "Just finished reading a great book about astrophysics. Highly recommend it!",
  //   date: new Date("2023-04-27").toUTCString(),
  //   comments: [],
  //   files: [],
  //   role: Role.NUMBER_1,
  // },
];

// export const userPosts: Post[] = [
//   {
//     id: adminPost1Id,
//     userId: adminId,
//     name: "Admin Nimda",
//     username: "ANimda",
//     avatar: await getAvatar(
//       "https://cdn.pixabay.com/photo/2020/06/24/19/12/cabbage-5337431_1280.jpg"
//     ),
//     upvotes: 20,
//     downvotes: 5,
//     text: "Just finished reading a great book about astrophysics. Highly recommend it!",
//     date: new Date("2023-05-1").toUTCString(),
//     comments: [],
//     files: [],
//   },
//   {
//     id: adminPost2Id,
//     userId: adminId,
//     name: "Admin Nimda",

//     username: "ANimda",
//     avatar: await getAvatar(
//       "https://cdn.pixabay.com/photo/2020/06/24/19/12/cabbage-5337431_1280.jpg"
//     ),
//     upvotes: 20,
//     downvotes: 5,
//     text: "Just finished reading a great book about astrophysics. Highly recommend it!",
//     date: new Date("2023-05-02T18:56:00").toUTCString(),
//     comments: [],
//     files: [],
//   },
// ];

// export const followedUsersPosts: Post[] = [
//   {
//     id: janePost3Id,
//     userId: janeId,
//     name: "Jane Doe",
//     username: "JaneDoe",
//     avatar: await getAvatar(
//       "https://avatars0.githubusercontent.com/u/9064066?v=4&s=460"
//     ),
//     upvotes: 15,
//     downvotes: 10,
//     text: "I'm planning a road trip across the US this summer. Any recommendations?",
//     date: new Date("2022-04-11").toUTCString(),
//     comments: [],
//     files: [],
//   },
//   {
//     id: johnPost4Id,
//     userId: johnId,
//     name: "John Doe",
//     username: "JDoe",
//     avatar: await getAvatar(
//       "https://cdn.pixabay.com/photo/2023/05/23/15/26/bengal-cat-8012976_960_720.jpg"
//     ),
//     upvotes: 20,
//     downvotes: 5,
//     text: "Just finished reading a great book about astrophysics. Highly recommend it!",
//     date: new Date("2023-04-27").toUTCString(),
//     comments: [],
//     files: [],
//   },
// ];

export const postsPerDayData = [10, 15, 7, 20, 12, 18, 25];
export const usersGrowthData = [10, 15, 7, 20, 12, 18, 25];
export const activeUsersData = [500, 600, 800, 700, 900, 1000];
export const ageDistributionData = [20, 30, 25, 15, 10];
export const genderDistributionData = [70, 30, 10];
