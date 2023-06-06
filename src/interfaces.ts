import { Gender, Role } from "./typescript-axios-generated";

export interface User {
  id: string;
  avatar?: File;
  role: Role;
  username: string;
  name: string;
  email: string;
  gender?: Gender;
  followers: string[];
  following: string[];
  bio?: string;
  likedPosts?: string[];
  dislikedPosts?: string[];
  createdAt: string;
  birthDate?: string;
  interests?: string[];
  locked: boolean;
}

export interface UserAdd {
  avatar?: File;
  role: Role;
  username: string;
  name: string;
  email: string;
  gender?: Gender;
  password: string;
  bio?: string;
  birthDate?: string;
  interests?: string[];
}

export interface UserEdit extends User {
  password?: string;
}

export interface Post {
  id: string;
  userId: string;
  avatar?: File;
  name: string;
  username: string;
  text?: string;
  upvotes: number;
  downvotes: number;
  date: string;
  comments?: Post[];
  files?: File[];
}

export interface PostAdd {
  userId: string;
  text?: string;
  files?: File[];
}

export interface AuthUser extends User {
  token: string;
  refreshToken: string;
  password: string;
}

// export interface RegisterDto {
//   username: string;
//   name: string;
//   email: string;
//   password: string;
// }

// export interface LoginDto {
//   email: string;
//   password: string;
// }

export interface NotificationAlert {
  id: string;
  type: "error" | "success" | "warning" | "info" | undefined;
  text: string;
}
