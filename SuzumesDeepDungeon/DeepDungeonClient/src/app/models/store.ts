export interface Store {
  id?: number;
  gameId?: number;
  rawgId?: string;
  storeId?: StoresEnum;
  url?: string;
  created?: Date | string;
  updated?: Date | string;
}

export enum StoresEnum {
  Steam = 1,
  GoG = 5,
  Nintendo = 6,
  Microsoft = 2,
  Playstation = 3,
  Xbox = 7,
  Epic = 11,
  Apple = 4,
  Google = 8,
  Itch = 9

  
}