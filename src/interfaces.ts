export interface User {
  id: number;
  avatar?: File;
  role: string;
  username: string;
  firstName: string;
  lastName: string;
  email: string;
  gender?: string;
  follower: number[];
  following: number[];
  bio?: string;
  liked: number[];
  disliked: number[];
  createdAt: Date;
  birthdate?: string;
  interests?: string[];
  locked: boolean;
}

export interface UserAdd {
  avatar?: File;
  role: string;
  username: string;
  firstName: string;
  lastName: string;
  email: string;
  gender?: string;
  password: string;
  bio?: string;
  birthdate?: string;
  interests?: string[];
}

export interface UserEdit {
  id: number;
  avatar?: File;
  role: string;
  username: string;
  firstName: string;
  lastName: string;
  email: string;
  gender?: string;
  password: string;
  follower: number[];
  following: number[];
  bio?: string;
  liked: number[];
  disliked: number[];
  birthdate?: string;
  interests?: string[];
  createdAt: Date;
}

export interface Post {
  id: number;
  userId: number;
  avatar?: File;
  firstName: string;
  lastName: string;
  username: string;
  text?: string;
  upvotes: number;
  downvotes: number;
  date: Date;
  comments?: Post[];
  files: File[];
}

export interface AddPost {
  userId: number;
  text?: string;
  files?: File[];
}

export interface AuthUser extends User {
  token: string;
  refreshToken: string;
  password: string;
}

export interface RegisterDto {
  username: string;
  firstName: string;
  lastName: string;
  email: string;
  password: string;
}

export interface LoginDto {
  email: string;
  password: string;
}
