using SuzumesDeepDungeon.Models;

namespace SuzumesDeepDungeon.DTO
{
    public class ExternalApiDTO
    {
            public int? Id { get; set; }
            public string? Name { get; set; }
            public string? Description { get; set; }
            public string? Key { get; set; }
            public int? UserId { get; set; }
            public UserDTO? User { get; set; }
            public bool? IsActive { get; set; }
        public DateTime? Created { get; set; }
    }
}
