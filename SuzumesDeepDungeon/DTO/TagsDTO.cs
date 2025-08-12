using SuzumesDeepDungeon.Models;
using System.Text.Json.Serialization;

namespace SuzumesDeepDungeon.DTO
{
    public class TagDTO
    {
        public int? Id { get; set; }
        public int? GameId { get; set; }
        public int? TagId { get; set; }
        public string? Name { get; set; }
        public string? Slug { get; set; }
        public string? Language { get; set; }
        public int? GamesCount { get; set; }
        public string? ImageBackground { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set; }
    }
}
