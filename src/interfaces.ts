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
  follower: number[];
  following: number[];
  bio?: string;
  liked: number[];
  disliked: number[];
  createdAt: Date;
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
  userId: number;
  text: string;
}

export interface AuthUser extends User {
  password: string;
  token: string;
}
