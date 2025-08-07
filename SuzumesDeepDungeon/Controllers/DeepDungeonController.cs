using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using SuzumesDeepDungeon.DTO;
using SuzumesDeepDungeon.Extensions;
using SuzumesDeepDungeon.Model;

namespace SuzumesDeepDungeon.Controllers;

[ApiController]
[Route("[controller]")]
public class DeepDungeon : ControllerBase
{
    private readonly ILogger<DeepDungeon> _logger;

    public DeepDungeon(ILogger<DeepDungeon> logger)
    {
        _logger = logger;
    }

    //CRUD SERVICE TO GET DATA
    [HttpGet(Name = "GetGameRank")]
    public IEnumerable<GameRankDTO> GetGameRank()
    {
        List<GameRankDTO> result = new List<GameRankDTO>();
        foreach (var item in GameRanks)
        {
            result.Add(GetDTO(item));
        }

        return result.OrderByDescending(x => x.updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGameRank(int id)
    {

        //сделать не удаление, а помечать это отдельным полем типа удалено, а потом выдавать отфильтрованные списки без этого поля
        var gameRank = GameRanks.FirstOrDefault(g => g.id == id);
        if (gameRank != null)
        {
            GameRanks.Remove(gameRank);
        }
        else
        {
            _logger.LogWarning($"GameRank with name {gameRank.name} not found.");
        }
        return NoContent();
    }

    [HttpPost(Name = "AddGameRank")]
    public GameRankDTO AddGameRank([FromBody] GameRankDTO newGameRank)
    {
        if (newGameRank == null || string.IsNullOrEmpty(newGameRank.name))
        {
            _logger.LogError("Invalid GameRank data provided.");
            return null;
        }
        var existingGameRank = GameRanks.FirstOrDefault(g => g.name.Equals(newGameRank.name, StringComparison.OrdinalIgnoreCase));
        if (existingGameRank != null)
        {
            _logger.LogWarning($"GameRank with name {newGameRank.name} already exists.");
            return null;
        }

        //Console.WriteLine(newGameRank.gameTime);
        var newIds = GameRanks.OrderBy(x => x.id);
        var Id = GameRanks.OrderByDescending(x => x.id).First().id;
        var newId = 1 + Id;


        var checkId = GameRanks.Where(x => x.id == newId).FirstOrDefault();

        if(checkId != null)
        {
            newId += 2;
        }

        var gameRank = new GameRank
        {
            id = newId,
            name = newGameRank.name,
            rate = newGameRank.rate ?? 0,
            status = (newGameRank.status != null ? (GameStatus)Enum.Parse(typeof(GameStatus), newGameRank.status) : GameStatus.Unknown),
            gameTime = TimeSpanExtensions.DecimalHoursToTimeSpan(newGameRank.gameTime ?? 0.0),
            review = newGameRank.review ?? "",
            created = DateTime.Now,
            updated = DateTime.Now,
            user = newGameRank.user ?? "Anonymous",
            image = newGameRank.image ?? "default.png"
        };

        GameRanks.Add(gameRank);
        _logger.LogInformation($"GameRank with name {newGameRank.name} added successfully with ID {gameRank.id}.");
        return GetDTO(gameRank);


    }

    [HttpPatch(Name = "UpdateGameRank")]
    public GameRankDTO UpdateGameRank([FromBody] GameRankDTO updatedGameRank)
    {
        if (updatedGameRank == null )
        {
            _logger.LogError("Invalid GameRank data provided.");
            return null;
        }
        var existingGameRank = GameRanks.FirstOrDefault(g => g.id == updatedGameRank.id);
        if (existingGameRank != null)
        {
            existingGameRank.name = updatedGameRank.name ?? existingGameRank.name;
            existingGameRank.rate = updatedGameRank.rate ?? existingGameRank.rate;
            existingGameRank.status = (updatedGameRank.status != null ? (GameStatus)Enum.Parse(typeof(GameStatus), updatedGameRank.status) : existingGameRank.status);
            existingGameRank.gameTime = TimeSpanExtensions.DecimalHoursToTimeSpan(updatedGameRank.gameTime ?? 0.0);
            existingGameRank.review = updatedGameRank.review ?? existingGameRank.review;
            existingGameRank.updated = DateTime.Now;
            existingGameRank.user = updatedGameRank.user ?? existingGameRank.user;
            existingGameRank.image = updatedGameRank.image ?? existingGameRank.image;

            _logger.LogInformation($"GameRank with name {updatedGameRank.name} updated successfully.");


        }
        else
        {
            _logger.LogWarning($"GameRank with name {updatedGameRank.name} not found.");
        }


        return GetDTO(existingGameRank);
    }

    //MockData


    private static List<GameRank> GameRanks = new List<GameRank>
    {
        new GameRank
        {
            id = 1,
            name = "Dark Souls",
            rate = 10,
            status = GameStatus.Completed,
            gameTime = new TimeSpan(8, 54, 0),
            review = "Great game!",
            created = DateTime.Now.AddDays(-20),
            updated = DateTime.Now.AddDays(-5),
            user = "User1",
            image = "assets/default-game.jpg"
        },
        new GameRank
        {
            id = 2,
            name = "Sekiro",
            rate = 8,
            status = GameStatus.OnHold,
            gameTime = new TimeSpan(45, 23, 0),
            review = "Enjoyable so far.",
            created = DateTime.Now.AddDays(-15),
            updated = DateTime.Now.AddDays(-2),
            user = "User2",
            image = "assets/default-game.jpg"
        }
    };
  

    private static GameRankDTO GetDTO(GameRank gameRank)
    {
        return new GameRankDTO
        {
            id = gameRank.id,
            name = gameRank.name,
            rate = gameRank.rate,
            status = gameRank.status.ToString(),
            gameTime = gameRank.gameTime.ToDecimalHours(),
            review = gameRank.review,
            created = gameRank.created,
            updated = gameRank.updated,
            user = gameRank.user,
            image = gameRank.image
        };
    }

}
