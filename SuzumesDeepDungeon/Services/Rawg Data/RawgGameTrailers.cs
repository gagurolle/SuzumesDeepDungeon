using System.Text.Json.Serialization;

namespace SuzumesDeepDungeon.Services.Rawg_Data
{
    public class GameVideosResponse
    {
        [JsonPropertyName("count")]
        public int Count { get; set; }

        [JsonPropertyName("next")]
        public string? Next { get; set; }

        [JsonPropertyName("previous")]
        public string? Previous { get; set; }

        [JsonPropertyName("results")]
        public List<GameVideo> Results { get; set; } = new List<GameVideo>();
    }

    public class GameVideo
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("preview")]
        public string PreviewImageUrl { get; set; } = string.Empty;

        [JsonPropertyName("data")]
        public VideoData Data { get; set; } = new VideoData();
    }

    public class VideoData
    {
        [JsonPropertyName("480")]
        public string Video480p { get; set; } = string.Empty;

        [JsonPropertyName("max")]
        public string VideoMaxQuality { get; set; } = string.Empty;
    }
}
