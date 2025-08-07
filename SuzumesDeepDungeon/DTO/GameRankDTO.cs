using SuzumesDeepDungeon.Model;

namespace SuzumesDeepDungeon.DTO
{
    public class GameRankDTO
    {
        public int id { get; set; }
        public string? name { get; set; }
        public int? rate { get; set; } // 10/10
        public string? status { get; set; }
        public double? gameTime { get; set; }
        public string? review { get; set; }
        public DateTime? created { get; set; }
        public DateTime? updated { get; set; }
        public string? user { get; set; }
        public string? image { get; set; }

    }
}
