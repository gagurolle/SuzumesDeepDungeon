using SuzumesDeepDungeon.Models;
using System.Text.Json.Serialization;

namespace SuzumesDeepDungeon.DTO
{
    public class TrailerDTO
    {
        public int? Id { get; set; }
        public int? GameId { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? PreviewImageUrl { get; set; } = string.Empty;
        public string? Video480p { get; set; } = string.Empty;
        public string? VideoMaxQuality { get; set; } = string.Empty;
        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set; }
    }
}
