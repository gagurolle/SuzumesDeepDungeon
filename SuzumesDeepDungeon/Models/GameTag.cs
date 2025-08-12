using System.Text.Json.Serialization;

namespace SuzumesDeepDungeon.Models
{
    public class GameTag
    {
            public int Id { get; set; }
            public int GameId { get; set; }
            public int TagId { get; set; }
            public string Name { get; set; }
            public string Slug { get; set; }
            public string Language { get; set; }
            public int GamesCount { get; set; }
            public string ImageBackground { get; set; }
        [JsonIgnore]
        public GameRank GameRank { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
    
}
