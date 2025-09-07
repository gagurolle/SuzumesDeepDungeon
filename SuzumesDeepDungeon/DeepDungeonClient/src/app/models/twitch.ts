// Базовый интерфейс с временными метками
export interface Timestamps {
  created: Date;
  updated: Date;
}

// Базовый интерфейс для сущностей с ID
export interface Entity {
  id: number;
}

// Базовая информация о пользователе
export interface BaseUserInfo extends Entity, Timestamps {
  userName?: string;
  userLogin?: string;
  userId: string;
  lastActive?: Date;
  previousActive?: Date;
}

// Полная информация о пользователе Twitch
export interface TwitchUser extends BaseUserInfo {
  description?: string;
  profileImageUrl?: string;
  userType?: string;
  isFollowing?: boolean;
  createdAt?: Date;
  accountAge?: number;
  game?: string;
  gameId?: string;
  channelTitle?: string;
  isSubscribed?: boolean;
  subscriptionTier?: string;
  isModerator?: boolean;
  isVip?: boolean;
}

// Системное действие
export interface TwitchSystemAction extends Entity, Timestamps {
  actionId?: string;
  actionName?: string;
}

// Базовый интерфейс для действий
export interface TwitchActionBase extends Entity, Timestamps {
  user?: TwitchUser;
  userId?: number;
  systemAction?: TwitchSystemAction;
  systemActionId?: number;
}

// Награда за redemption
export interface TwitchRewardRedemption extends TwitchActionBase {
  rewardName?: string;
  rewardPrompt?: string;
  rewardCost?: string;
  counter?: string;
  userCounter?: boolean;
}

// Триггер команды
export interface TwitchCommandTriggered extends TwitchActionBase {
  command?: string;
  commandName?: string;
  commandSource?: string;
  commandType?: string;
  isReply?: boolean;
}

// Основное действие Twitch
export interface TwitchAction extends Entity {
  user: TwitchUser;
  systemAction: TwitchSystemAction;
  rewardRedemption?: TwitchRewardRedemption;
  commandTriggered?: TwitchCommandTriggered;
  rowData: string;
}