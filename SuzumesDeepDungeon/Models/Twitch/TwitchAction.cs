namespace SuzumesDeepDungeon.Models.Twitch
{
    public class TwitchAction
    {
        public int Id { get; set; }
        public TwitchUser? User { get; set; }
        public int? UserId { get; set; }
        public TwitchSystemAction? SystemAction { get; set; }
        public int? SystemActionId { get; set; }
        public TwitchRewardRedemption? RewardRedemption { get; set; }

        public int? RewardRedemptionId { get; set; }
        public TwitchCommandTriggered? CommandTriggered { get; set; }

        public int? CommandTriggeredId { get; set; }
        public string? RowData { get; set; } 
    }
}
