import { GameAchievement } from "./game-achievement";
import { GameStatus } from "./game-status.enum";
import { Screenshot } from "./screenshot";
import { Store } from "./store";
import { GameTag } from "./tags";
import { Trailer } from "./trailer ";
import { User } from "./user";

export interface GameRankDTO {
  id: number;
  name?: string | null;
  rate?: number | null;
  status: GameStatus;
  gameTime?: number | null;
  review?: string | null;
  created?: Date | string | null;
  updated?: Date | string | null;
  user?: User | null;
  image?: string | null;
  youtubeLink?: string | null;
  metacriticRate?: number | null;
  released?: string | null;
  stores?: Store[] | null;
  screenshots?: Screenshot | null;
  achievements?: GameAchievement[] | null;
  trailers?: Trailer[] | null;
  tags?: GameTag[] | null;
  rawgId?: string | null;
}