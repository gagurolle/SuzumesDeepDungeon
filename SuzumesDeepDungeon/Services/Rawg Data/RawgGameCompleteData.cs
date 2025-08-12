namespace SuzumesDeepDungeon.Services.Rawg_Data
{
    public class RawgGameCompleteData
    {
       public Game game { get; set; }
       public GameVideosResponse trailers { get; set; }
       public AchievementsApiResponse achievements { get; set; }
       public StoreApiResponse stores { get; set; } 
        public string? steamUrl { get;  set; }
        public string? steamJpg { get; set; }
    }
}
