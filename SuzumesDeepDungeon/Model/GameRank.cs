using Microsoft.AspNetCore.Mvc.ViewEngines;
using static System.Net.Mime.MediaTypeNames;
using System;

namespace SuzumesDeepDungeon.Model
{
    public class GameRank
    {
        public int id { get; set; }
        public string name { get; set; }
        public int rate { get; set; } // 10/10
        public GameStatus status { get; set; }
        public TimeSpan gameTime { get; set; }
        public string? review { get; set; }
        public DateTime created { get; set; }
        public DateTime updated { get; set; }
        public string user { get; set; }
        public string? image { get; set; }

    }
}
