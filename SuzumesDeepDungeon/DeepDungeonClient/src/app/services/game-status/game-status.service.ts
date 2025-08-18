import { Injectable } from "@angular/core";
import { GameStatus } from "../../models/game-status.enum";
import { Store, StoresEnum } from "../../models/store";

// services/game-status.service.ts
@Injectable({ providedIn: 'root' })
export class GameStatusService {
  private statusTranslations: Record<GameStatus, string> = {
      [GameStatus.InProgress]: 'В процессе',
      [GameStatus.Completed]: 'Пройдено',
      [GameStatus.Drop]: "Дроп",
      [GameStatus.OnHold]: "Отложено",
      [GameStatus.PlantoPlay]: "Планирую играть",
      [GameStatus.NetworkGame]: "Сетевая игра",
      [GameStatus.Unknown]: "Неизвестно"
  };

  getStatusText(status: GameStatus): string {
   
    return this.statusTranslations[status] || status;
  }

  getAllStatuses(): { value: GameStatus, text: string }[] {
    return Object.values(GameStatus).map(value => ({
      value,
      text: this.statusTranslations[value]
    }));
  }

  getStoreName(store: Store | undefined): string {
      if (!store?.storeId) return 'Неизвестный магазин';
      
      switch(store.storeId) {
        case StoresEnum.Steam: return 'Steam';
        case StoresEnum.GoG: return 'GOG';
        case StoresEnum.Nintendo: return 'Nintendo';
        case StoresEnum.Microsoft: return 'Microsoft Store';
        case StoresEnum.Playstation: return 'PlayStation Store';
        case StoresEnum.Xbox: return 'Xbox Store';
        case StoresEnum.Epic: return 'Epic Games Store';
        case StoresEnum.Apple: return 'Apple Store';
        case StoresEnum.Google: return 'Google Play Store';
        case StoresEnum.Itch: return 'Itch.io';
        default: return store.storeId || 'Неизвестный магазин';
      }
    }
}