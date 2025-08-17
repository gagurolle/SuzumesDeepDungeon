using static SuzumesDeepDungeon.Models.User;

namespace SuzumesDeepDungeon.DTO
{
    public class RegistrationDTO
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public bool? IsAdmin { get; set; }
    }
}
