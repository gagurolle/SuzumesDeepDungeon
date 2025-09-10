import { User } from "./user";

export interface MinecraftContent {
  id?: number;
  userId?: number;
  user?: User;
  header?: string;
  content?: string;
  created?: Date;
  updated?: Date;
}