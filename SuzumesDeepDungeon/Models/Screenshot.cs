using System.Text.Json.Serialization;

namespace SuzumesDeepDungeon.Models
{
    public class Screenshot
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public string? SteamHeaderUrl { get; set; } = "";
        public string? SteamCapsuleUrl { get; set; } = "";
        public string? Steam600x900Url { get; set; } = "";
        public string? RawgBackgroundUrl { get; set; } = "";
        [JsonIgnore]
        public GameRank? GameRank { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
