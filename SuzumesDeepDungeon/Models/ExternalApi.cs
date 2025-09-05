namespace SuzumesDeepDungeon.Models
{
    public class ExternalApi
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string HashKey { get; set; }
        public User? User { get; set; }
        public bool IsActive { get; set; } = true;

        public int? UserId { get; set; }
        public DateTime Created { get; set; }
    }
}
