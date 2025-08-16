export interface Screenshot {
  id?: number;
  gameId?: number;
  steamHeaderUrl?: string;
  steamCapsuleUrl?: string;
  steam600x900Url?: string;
  rawgBackgroundUrl?: string;
  created?: Date | string;
  updated?: Date | string;
}