using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Microsoft.IdentityModel.Tokens;
using SuzumesDeepDungeon.Data;
using SuzumesDeepDungeon.DTO;
using SuzumesDeepDungeon.Extensions;
using SuzumesDeepDungeon.Models;
using SuzumesDeepDungeon.Services;
using SuzumesDeepDungeon.Services.CSVLoad;
using SuzumesDeepDungeon.Services.HowLongToBeat;
using SuzumesDeepDungeon.Services.Rawg_Data;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text;

namespace SuzumesDeepDungeon.Controllers
{
    [ApiController]
    [Route("api/gameService")]
    public class GameServiceController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly DatabaseContext _context;

        public GameServiceController(IConfiguration config, DatabaseContext context)
        {
            _config = config;
            _context = context;
        }

        [HttpGet("csvLoad1Game")]
        public async Task<ActionResult<List<TwitchStatisticGames>>> CSVGameLoad1(string path, string game = null)
        {
            var k = CSVLoad.LoadGames(path);
            TwitchStatisticGames? p = null;
            var user = _context.Users.Where(x => x.IsAdmin == true).FirstOrDefault();
            if (game != null && !string.IsNullOrEmpty(game))
            {
                 p = k.Where(x => x.gameName == game).FirstOrDefault();
                if (p == null)
                {
                    return NotFound("Game not found in the provided CSV file.");
                }
            }
            else
            {
                 p = k.FirstOrDefault();
            }

                List<GameRank> gameRanks = new List<GameRank>();
            List<Exception> exceptions = new();

                var existingGame = _context.GameRanks.FirstOrDefault(g => g.Name == p.gameName);
                if (existingGame != null)
                {
                   return BadRequest("Game already exists in the database.");
            }
                Task.Delay(100).Wait();
                try
                {
                    var rawgresponse = await GameAddService.RawgAPI.GetGameFullInfo(p.gameName);
                    if (rawgresponse == null)
                    {
                         return NotFound("Game data not found in RAWG API.");
                }
                    var gameRankData = await GameAddService.RawgAPI.RawgDataToGameRank(rawgresponse, user);
                    if (gameRankData == null)
                    {
                    return NotFound("Game data not found in RAWG API.");
                }
                    gameRankData.GameTime = TimeSpanExtensions.DecimalHoursToTimeSpan(p.StreamTime);
                    gameRankData.Created = p.LastSeen;
                    gameRanks.Add(gameRankData);
                _context.GameRanks.Add(gameRankData);
                await _context.SaveChangesAsync();
            }
                catch (Exception e)
                {
                    exceptions.Add(e);
                }

            return Ok(p);
        }


        [HttpGet("csvLoad")]
        public async Task<ActionResult<List<TwitchStatisticGames>>> CSVGameLoad(string path)
        {
            var p = CSVLoad.LoadGames(path);
            var user = _context.Users.Where(x => x.IsAdmin == true).FirstOrDefault();
            List<GameRank> gameRanks = new List<GameRank>();
            Dictionary<Exception, TwitchStatisticGames?> exceptions = new();
            foreach (var item in p)
            {
                var existingGame = _context.GameRanks.FirstOrDefault(g => g.Name == item.gameName);
                if (existingGame != null)
                {
                    continue; // Game already exists, skip to the next one
                }
                Task.Delay(100).Wait();
                try
                {
                    var rawgresponse = await GameAddService.RawgAPI.GetGameFullInfo(item.gameName);
                    if (rawgresponse == null)
                    {
                        continue;
                    }
                    var gameRankData = await GameAddService.RawgAPI.RawgDataToGameRank(rawgresponse, user);
                    if (gameRankData == null)
                    {
                        continue;
                    }
                    gameRankData.GameTime = TimeSpanExtensions.DecimalHoursToTimeSpan(item.StreamTime);
                    gameRankData.Created = item.LastSeen;
                    gameRanks.Add(gameRankData);

                    _context.GameRanks.Add(gameRankData);
                    await _context.SaveChangesAsync();
                }
                catch(Exception e)
                {
                    exceptions.Add(e, item);
                }
                
            }

            foreach(var exception in exceptions)
            {
                Console.WriteLine($"Error processing game '{exception.Value?.gameName}': {exception.Key.InnerException}");
            }
            return Ok(p);
        }


        [HttpPost("addGame")]
        public async Task<ActionResult<GameRank>> AddGame([FromBody] string gameName)
        {

            
            var t = await GameAddService.RawgAPI.GetGameFullInfo(gameName);

            var p = GameAddService.RawgAPI.RawgDataToGameRank(t).Result;
            
            var user = _context.Users.FirstOrDefault(x => x.Username == "pansuzumi");

            p.User = user;

            var existingGame = _context.GameRanks.FirstOrDefault(g => g.Name == p.Name);
            if (existingGame != null)
            {
                return BadRequest("Game already exists in the database.");
            }

            _context.GameRanks.Add(p);

            await _context.SaveChangesAsync();
            return Ok("Ok");
        }

        [HttpGet("getHLTBId")]
        public async Task<ActionResult<HltbSearchResult>> GetId(string gameName)
        {
            try
            {
                using var httpClient = new HttpClient();

                var result = await HowLongToBeatSeeker.SeekGame(gameName, httpClient);

                Console.WriteLine($"Found {result.Count} results:");
                foreach (var game in result.Data)
                {
                    Console.WriteLine($"\nID: {game.GameId}");
                    Console.WriteLine($"Name: {game.GameName}");
                    Console.WriteLine($"Platforms: {game.ProfilePlatform}");
                    Console.WriteLine($"Main Story: {TimeSpan.FromSeconds(game.CompMain):h\\:mm} hours");
                    Console.WriteLine($"Completionist: {TimeSpan.FromSeconds(game.Comp100):h\\:mm} hours");
                    Console.WriteLine($"Steam ID: {game.ProfileSteam}");
                    Console.WriteLine($"Image: {HltbSearch.IMAGE_URL}{game.GameImage}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return NotFound(ex.Message);
            }

           return NotFound("Game not found in HowLongToBeat database.");


        }

        [HttpGet("findGameData")]
        public async Task<ActionResult<List<FindGameDTO>>> FindGameData(string gameName)
        {
            var games = await GameAddService.RawgAPI.FindGames(gameName);
            // обработать null
            List<FindGameDTO> gamesDTO = new List<FindGameDTO>();
            foreach(var ent in games)
            {
                gamesDTO.Add(new FindGameDTO()
                {
                    Id = ent.Id,
                    Slug= ent.Slug,
                    Name = ent.Name,
                    Released = ent.Released,
                    BackgroundImage = ent.BackgroundImage
                });
            }
            return Ok(gamesDTO);
        }



        [HttpPost("getGameData")]
        public async Task<ActionResult<GameRankDTO>> GetDataFromRawg([FromBody] FindGameDTO game)
        {
           var t = await GameAddService.RawgAPI.GetGameFullInfo(gameName: game.Name,id: game.Id.ToString());

           var p = GameAddService.RawgAPI.RawgDataToGameRank(t).Result;

            return Ok(p);
        }
    }
}
