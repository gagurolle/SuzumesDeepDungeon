export interface User {
  username: string;
  email: string;
  isAdmin: boolean;
}

export enum UserRole {
  Admin = 'Admin',
  User = 'User'
}