namespace SuzumesDeepDungeon.Models.Minecraft;

public class MinecraftContent
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
   public string Header { get; set; }
   public string Content { get; set; }
   public DateTime Created { get; set; }
   public DateTime Updated { get; set; }
}