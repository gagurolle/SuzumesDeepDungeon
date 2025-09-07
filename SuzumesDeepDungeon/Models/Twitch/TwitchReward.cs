namespace SuzumesDeepDungeon.Models.Twitch
{
    public class TwitchRewardRedemption : TwitchActionBase
    {
        public string? RewardName { get; set; }
        public string? RewardPrompt { get; set; }
        public string? RewardCost { get; set; }
        public string? Counter { get; set; }
        public bool? UserCounter { get; set; }
    }
}
