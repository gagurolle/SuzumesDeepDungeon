namespace SuzumesDeepDungeon.Models.Twitch
{
    public class TwitchSystemAction
    {
        public int Id { get; set; }
        public Guid? ActionId { get; set; }
        public string? ActionName { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
