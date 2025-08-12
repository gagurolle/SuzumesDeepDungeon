using SuzumesDeepDungeon.Models;
using System.Text.Json.Serialization;

namespace SuzumesDeepDungeon.DTO
{
    public class ScreenshotDTO
    {
        public int? Id { get; set; }
        public int? GameId { get; set; }
        public string? SteamHeaderUrl { get; set; } = string.Empty;
        public string? SteamCapsuleUrl { get; set; } = string.Empty;
        public string? Steam600x900Url { get; set; } = string.Empty;
        public string? RawgBackgroundUrl { get; set; } = string.Empty;

        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set; }
    }
}
