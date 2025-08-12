using SuzumesDeepDungeon.Models;

namespace SuzumesDeepDungeon.DTO
{
    public class GameRankDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? RawgId { get; set; } = string.Empty;
        public int? Rate { get; set; } // 10/10
        public string? Status { get; set; }
        public double? GameTime { get; set; }
        public string? Review { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set; }
        public UserDTO? User { get; set; }
        public string? Image { get; set; }
        public string? YoutubeLink { get; set; } = string.Empty;
        public double? MetacriticRate { get; set; }
        public string? Released { get; set; }
        public List<StoreDTO>? Stores { get; set; } = new List<StoreDTO>();
        public ScreenshotDTO? Screenshots { get; set; }
        public List<AchievementDTO>? Achievements { get; set; } = new List<AchievementDTO>();
        public List<TrailerDTO>? Trailers { get; set; } = new List<TrailerDTO>();
        public List<TagDTO>? Tags { get; set; } = new List<TagDTO>();
    }
}
