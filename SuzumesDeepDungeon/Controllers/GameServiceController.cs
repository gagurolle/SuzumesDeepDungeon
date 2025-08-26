using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly DatabaseContext _context;
        private readonly ILogger<GameServiceController> _logger;
        private readonly RawgApi _rawgApi;
        private readonly CSVLoad _csvLoad;

        public GameServiceController(ILogger<GameServiceController> logger, DatabaseContext context, RawgApi rawgApi, CSVLoad csvLoad)
        {
            _logger = logger;
            _context = context;
            _rawgApi = rawgApi;
            _csvLoad = csvLoad;

        }
        [Authorize(Roles = "Admin")]
        [HttpGet("csvLoad")]
        public async Task<ActionResult<List<TwitchStatisticGames>>> CSVGameLoad(string path)
        {
            var p = _csvLoad.LoadGames(path);
            var user = _context.Users.Where(x => x.IsAdmin == true).FirstOrDefault();
            List<GameRank> gameRanks = new List<GameRank>();
            Dictionary<Exception, TwitchStatisticGames?> exceptions = new();
            foreach (var item in p)
            {
                var existingGame = _context.GameRanks.FirstOrDefault(g => EF.Functions.ILike(g.Name, item.gameName));
                if (existingGame != null)
                {
                    continue;
                }
                Task.Delay(100).Wait();
                try
                {
                    var rawgresponse = await _rawgApi.GetGameFullInfo(item.gameName);
                    if (rawgresponse == null)
                    {
                        continue;
                    }
                    var gameRankData = await _rawgApi.RawgDataToGameRank(rawgresponse, user);
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
                _logger.LogWarning($"Error processing game '{exception.Value?.gameName}': {exception.Key.InnerException}");
            }
            return Ok(p);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("addGame")]
        public async Task<ActionResult<GameRank>> AddGame([FromBody] string gameName)
        {
            var t = await _rawgApi.GetGameFullInfo(gameName);

            var p = _rawgApi.RawgDataToGameRank(t).Result;
            
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        [HttpGet("findGameData")]
        public async Task<ActionResult<List<FindGameDTO>>> FindGameData(string gameName)
        {
            var games = await _rawgApi.FindGames(gameName);
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

        [Authorize(Roles = "Admin")]
        [HttpPost("getGameData")]
        public async Task<ActionResult<GameRankDTO>> GetDataFromRawg([FromBody] FindGameDTO game)
        {
           var t = await _rawgApi.GetGameFullInfo(gameName: game.Name,id: game.Id.ToString());

           var p = _rawgApi.RawgDataToGameRank(t).Result;

            return Ok(p);
        }
    }
}
