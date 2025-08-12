using System.Text.Json.Serialization;

namespace SuzumesDeepDungeon.DTO
{
    public class FindGameDTO
    {
            public int? Id { get; set; }
            public string? Slug { get; set; }
            public string? Name { get; set; }
            public string? NameOriginal { get; set; }
            public string? Description { get; set; }
            public string? Released { get; set; }
            public string? BackgroundImage { get; set; }
            public string? Website { get; set; }
        }
}
