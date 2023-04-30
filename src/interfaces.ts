export interface User {
  id: number;
  role: string;
  username: string;
  firstName: string;
  lastName: string;
  email: string;
  gender?: string;
  password: string;
}
