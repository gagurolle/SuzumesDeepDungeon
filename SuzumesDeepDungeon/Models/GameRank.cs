using SuzumesDeepDungeon.DTO;
using SuzumesDeepDungeon.Extensions;

namespace SuzumesDeepDungeon.Models
{
    public class GameRank
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string RawgId { get; set; } = string.Empty; // Unique identifier for the game in RAWG
        public double Rate { get; set; } // 10/10
        public GameStatus Status { get; set; }
        public TimeSpan GameTime { get; set; }
        public string? Review { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public int? UserId { get; set; }
        public User? User { get; set; } = null!;
        public string? Image { get; set; } = string.Empty;
        public string? YoutubeLink{ get; set; } = string.Empty;
        public double? MetacriticRate { get; set; }
        public string? Released { get; set; }
        public List<Stores> Stores { get; set; } = new List<Stores>();
        public Screenshot? Screenshots { get; set; }
        public List<GameAchievement> Achievements { get; set; } = new List<GameAchievement>();
        public List<Trailer> Trailers { get; set; } = new List<Trailer>();
        public List<GameTag> Tags { get; set; } = new List<GameTag>();
       

    }


}
