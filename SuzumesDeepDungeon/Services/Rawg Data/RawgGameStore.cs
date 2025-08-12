using System.Text.Json.Serialization;

namespace SuzumesDeepDungeon.Services.Rawg_Data
{
    public class StoreApiResponse
    {
        [JsonPropertyName("count")]
        public int Count { get; set; }

        [JsonPropertyName("next")]
        public string? Next { get; set; }

        [JsonPropertyName("previous")]
        public string? Previous { get; set; }

        [JsonPropertyName("results")]
        public List<StoreResult> Results { get; set; } = new List<StoreResult>();
    }

    public class StoreResult
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("game_id")]
        public int GameId { get; set; }

        [JsonPropertyName("store_id")]
        public int StoreId { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;
    }
}
