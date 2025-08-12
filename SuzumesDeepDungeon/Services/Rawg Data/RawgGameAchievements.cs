using System.Text.Json.Serialization;

namespace SuzumesDeepDungeon.Services.Rawg_Data
{
    public class AchievementsApiResponse
    {
        [JsonPropertyName("count")]
        public int Count { get; set; }

        [JsonPropertyName("next")]
        public string? Next { get; set; }

        [JsonPropertyName("previous")]
        public string? Previous { get; set; }

        [JsonPropertyName("results")]
        public List<Achievement> Results { get; set; } = new List<Achievement>();
    }

    public class Achievement
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("image")]
        public string ImageUrl { get; set; } = string.Empty;

        [JsonPropertyName("percent")]
        public string CompletionPercent { get; set; } = string.Empty;
    }
}
