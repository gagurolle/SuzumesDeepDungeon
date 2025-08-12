using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Microsoft.IdentityModel.Tokens;
using SuzumesDeepDungeon.Data;
using SuzumesDeepDungeon.DTO;
using SuzumesDeepDungeon.Models;
using SuzumesDeepDungeon.Services;
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
