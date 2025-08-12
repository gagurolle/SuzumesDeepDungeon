using SuzumesDeepDungeon.Models;
using System.Text.Json.Serialization;

namespace SuzumesDeepDungeon.DTO
{
    public class StoreDTO
    {
        public int? Id { get; set; }
        public int? GameId { get; set; }
        public string? RawgId { get; set; }
        public StoresEnum? StoreId { get; set; }
        public string? Url { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set; }
    }
}
