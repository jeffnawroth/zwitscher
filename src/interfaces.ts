export interface User {
  id: number;
  avatar?: string;
  role: string;
  username: string;
  firstName: string;
  lastName: string;
  email: string;
  gender?: string;
  password: string;
  follower: number;
  following: number;
}

export interface Post {
  id: number;
  userId: number;
  avatar?: string;
  firstName: string;
  lastName: string;
  username: string;
  text: string;
  upvotes: number;
  downvotes: number;
  date: Date;
}

export interface AddPost {
  firstName: string;
  avatar: string;
  lastName: string;
  username: string;
  text: string;
  date: Date;
}

export interface AuthUser {
  id: number;
  avatar?: string;
  role: string;
  username: string;
  firstName: string;
  lastName: string;
  email: string;
  gender?: string;
  password: string;
  token: string;
  follower: number;
  following: number;
  liked: number[];
  disliked: number[];
}
