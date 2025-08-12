using System.Text.Json.Serialization;

namespace SuzumesDeepDungeon.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; } // e.g., "admin", "user"
        public bool IsAdmin { get; set; }
        [JsonIgnore] 
        public List<GameRank> GameRanks { get; set; } = new List<GameRank>();
        public DateTime Created { get; set; } // Date when the user was created
        public DateTime Updated { get; set; } // Date when the user was last updated
    }

    public enum UserRole
    {
        Admin,
        User
    }
}
