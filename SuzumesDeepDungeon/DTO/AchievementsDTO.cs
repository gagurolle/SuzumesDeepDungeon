using SuzumesDeepDungeon.Models;

namespace SuzumesDeepDungeon.DTO
{
    public class AchievementDTO
    {
        public int? Id { get; set; }
        public int? GameId { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public string? ImageUrl { get; set; } = string.Empty;
        public string? CompletionPercent { get; set; } = string.Empty;
        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set; }
    }
}
