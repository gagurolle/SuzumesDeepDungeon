namespace SuzumesDeepDungeon.Models.Twitch
{
    public class TwitchCommandTriggered : TwitchActionBase
    {
        public string? Command { get; set; }
        public string? CommandName { get; set; }
        public string? CommandSource { get; set; }
        public string? CommandType { get; set; }
        public bool? IsReply { get; set; }
    }
}
