using System.Text.Json.Serialization;

namespace SuzumesDeepDungeon.Models
{

    public class GameAchievement
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string CompletionPercent { get; set; } = string.Empty;
        [JsonIgnore]
        public GameRank GameRank { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
