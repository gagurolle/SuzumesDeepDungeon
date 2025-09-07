namespace SuzumesDeepDungeon.Models.Twitch
{
    public class TwitchMoney: TwitchActionBase
    {
        public string Message { get; set; } = null!;
        public string Currency { get; set; } = null!;
        public int Amount { get; set; }
        public string Service { get; set; } = null!;

    }
}
