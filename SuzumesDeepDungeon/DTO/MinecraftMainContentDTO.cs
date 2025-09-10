namespace SuzumesDeepDungeon.Models.Minecraft;

public class MinecraftMainContent
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public string? Header { get; set; }
    public string? HeaderInfo { get; set; }
    public string? Adres { get; set; }
    public string? Version { get; set; }
    public string? Mod { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
}