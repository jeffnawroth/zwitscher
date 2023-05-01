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
}

export interface Post {
  id: number;
  avatar?: string;
  firstName: string;
  lastName: string;
  username: string;
  text: string;
  upvotes: number;
  downvotes: number;
}

export interface AddPost {
  firstName: string;
  avatar: string;
  lastName: string;
  username: string;
  text: string;
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
}
