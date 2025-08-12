using System.Text.Json.Serialization;

namespace SuzumesDeepDungeon.Models
{
    public class Screenshot
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public string SteamHeaderUrl { get; set; } = string.Empty;
        public string SteamCapsuleUrl { get; set; } = string.Empty;
        public string Steam600x900Url { get; set; } = string.Empty;
        public string RawgBackgroundUrl { get; set; } = string.Empty;
        [JsonIgnore]
        public GameRank? GameRank { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
