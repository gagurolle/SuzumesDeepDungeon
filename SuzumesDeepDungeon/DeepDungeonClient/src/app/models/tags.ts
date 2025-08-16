export interface GameTag {
  id?: number;
  gameId?: number;
  tagId?: number;
  name?: string;
  slug?: string;
  language?: string;
  gamesCount?: number;
  imageBackground?: string;
  created?: Date | string;
  updated?: Date | string;
}