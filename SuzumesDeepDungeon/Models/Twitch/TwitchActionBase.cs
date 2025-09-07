namespace SuzumesDeepDungeon.Models.Twitch
{
    public class TwitchActionBase
    {
        public int Id { get; set; }
        public TwitchUser? User { get; set; } = null!;
        public int? UserId { get; set; }
        public TwitchSystemAction? SystemAction { get; set; } = null!;

        public int? SystemActionId { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
