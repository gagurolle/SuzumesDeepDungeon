namespace SuzumesDeepDungeon.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } // e.g., "admin", "user"
        public bool IsAdmin { get; set; }
        public DateTime CreatedAt { get; set; } // Date when the user was created
        public DateTime UpdatedAt { get; set; } // Date when the user was last updated
    }
}
