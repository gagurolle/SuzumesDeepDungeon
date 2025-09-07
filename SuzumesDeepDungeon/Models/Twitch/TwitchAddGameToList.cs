namespace SuzumesDeepDungeon.Models.Twitch
{
    public class TwitchAddGameToList : TwitchActionBase
    {
        //public TwitchReward? Reward { get; set; } = null!;
        public int? RewardId { get; set; }
       // public TwitchCommand? Command { get; set; } = null!;
        public int? CommandId { get; set; }
        public TwitchMoney TwitchMoney { get; set; } = null!;
        public int? TwitchMoneyId { get; set; }
        public string? GameName { get; set; }
    }
}
