export interface GameAchievement {
  id: number;
  gameId: number;
  name: string;
  description: string;
  imageUrl: string;
  completionPercent: string;
  created: Date | string;
  updated: Date | string;
}