using System.Text.Json.Serialization;

namespace SuzumesDeepDungeon.DTO.Twitch
{
    [Serializable]
    public class ActionBaseDTO
    {
        public string? rawInput { get; set; }
        public string? rawInputEscaped { get; set; }
        public string? triggerName { get; set; }
        public string? triggerCategory { get; set; }
        public string? actionName { get; set; }
        public string? source { get; set; }
    }
    [Serializable]
    public class CommandTriggeredDTO : ActionBaseDTO
    {
        public string? command { get; set; }
        public string? commandName { get; set; }
        public string? commandSource { get; set; }
        public string? commandType { get; set; }
        public bool? isReply { get; set; }
    }
    [Serializable]
    public class TwitchRewardRedemptionDTO : ActionBaseDTO
    {
        public string? rewardName { get; set; }
        public string? rewardPrompt { get; set; }
        public string? rewardCost { get; set; }
        public string? counter { get; set; }
        public bool? userCounter { get; set; }


    }
    [Serializable]
    public class AddGameInput
    {
        [JsonPropertyName("userInfoEx")]
        public UserInfoEx? userInfoEx { get; set; }
        [JsonPropertyName("commandTriggered")]
        public CommandTriggeredDTO? commandTriggered { get; set; }
        [JsonPropertyName("twitchRewardRedemption")]
        public TwitchRewardRedemptionDTO? twitchRewardRedemption { get; set; }

        [JsonPropertyName("action")]
        public TwitchSystemActionDTO action { get; set; }
        [JsonPropertyName("gameName")]
        public string? gameName { get; set; } = null!;
    }
    [Serializable]
    public class TwitchSystemActionDTO
    {
        public int? Id { get; set; }
        public Guid? ActionId { get; set; }
        public string? ActionName { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set; }
    }

    [Serializable]
    public class BaseUserInfoDTO
    {
        public string? UserName { get; set; }
        public string? UserLogin { get; set; }
        public string? UserId { get; set; } = null!;
        public DateTime? LastActive { get; set; }
        public DateTime? PreviousActive { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set; }
    }


    [Serializable]
    public class UserInfoEx : BaseUserInfoDTO
    {

        public string? Description { get; set; }

        public string? ProfileImageUrl { get; set; }

        public string? UserType { get; set; }

        public bool? IsPartner => string.Equals(UserType, "partner", StringComparison.OrdinalIgnoreCase);

        public bool? IsAffiliate => string.Equals(UserType, "affiliate", StringComparison.OrdinalIgnoreCase);

        public bool? IsFollowing { get; set; }

        public DateTime? CreatedAt { get; set; }

        public double? AccountAge { get; set; }

        public string? Game { get; set; }

        public string? GameId { get; set; }

        public string? ChannelTitle { get; set; }

        public List<string>? Tags { get; set; }

        public bool? IsSubscribed { get; set; }

        public string? SubscriptionTier { get; set; }

        public bool? IsModerator { get; set; }

        public bool? IsVip { get; set; }
    }

}
