import { User } from "./user";

export interface ExternalApi {
  id?: number;
  name?: string;
  description?: string;
  key?: string;
  userId?: number;
  user?: User;
  isActive?: boolean;
  created?: Date | string; // В зависимости от способа обработки дат
}