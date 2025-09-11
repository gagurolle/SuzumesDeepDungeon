import { User } from "./user";

export interface MinecraftMainContent {
  id?: number;
  userId?: number;
  user?: User;
  header?: string;
  headerInfo?: string;
  adres?: string;
  version?: string;
  mod?: string;
  created?: Date;
  updated?: Date;
}