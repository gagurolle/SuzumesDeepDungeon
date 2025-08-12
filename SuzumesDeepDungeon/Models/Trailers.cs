using System.Text.Json.Serialization;

namespace SuzumesDeepDungeon.Models
{
    public class Trailer
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string PreviewImageUrl { get; set; } = string.Empty;
        public string Video480p { get; set; } = string.Empty;
        public string VideoMaxQuality { get; set; } = string.Empty;
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        [JsonIgnore]
        public GameRank GameRank { get; set; }
    }

   
}
