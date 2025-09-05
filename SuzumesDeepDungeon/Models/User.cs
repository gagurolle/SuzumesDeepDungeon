using System.Text.Json.Serialization;

namespace SuzumesDeepDungeon.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
        public bool IsAdmin { get; set; }
        [JsonIgnore]
        public List<GameRank> GameRanks { get; set; } = new List<GameRank>();

        public List<ExternalApi> ExternalApi { get; set; } = new List<ExternalApi>();
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public enum UserRole
        {
            Admin,
            User
        }
    }
}
