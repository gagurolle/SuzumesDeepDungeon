using System.Text.Json.Serialization;

namespace SuzumesDeepDungeon.Models
{
    public class Stores
    {
       public int Id { get; set; }
       public int GameId { get; set; }
       public string RawgId { get; set; }
       public StoresEnum StoreId { get; set; }
       public string Url { get; set; }
        [JsonIgnore]
        public GameRank GameRank { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }

    public enum StoresEnum
    {
        Steam = 1,
        GoG = 5,
        Nintendo = 6,
        Microsoft = 2,
        Playstation = 3,
        Xbox = 7
    }
}
