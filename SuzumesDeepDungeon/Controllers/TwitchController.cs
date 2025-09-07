using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuzumesDeepDungeon.Attributes;
using SuzumesDeepDungeon.Data;
using SuzumesDeepDungeon.DTO;
using SuzumesDeepDungeon.DTO.Twitch;
using SuzumesDeepDungeon.Models.Twitch;
using SuzumesDeepDungeon.Services;
using System.Diagnostics;

namespace SuzumesDeepDungeon.Controllers
{
    [Route("api/twitch")]
    [ApiController]
    public class TwitchController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly DatabaseContext _context;
        private readonly TwitchService _twitch;
        private readonly ILogger<DeepDungeon> _logger;

        public TwitchController(ILogger<DeepDungeon> logger, IConfiguration config, DatabaseContext context, TwitchService twitch)
        {
            _config = config;
            _context = context;
            _twitch = twitch;
            _logger = logger;
        }

        [ApiKeyAuth]
        [HttpPost("addGame")]
        public async Task<IActionResult> AddGame([FromBody] AddGameInput info)
        {

            await _twitch.AddGameToList(info);

            _logger.LogWarning(info.gameName + "  -   " + info?.userInfoEx?.UserName);
            return Ok(info.gameName);
        }
        [HttpPost("deleteGameFromList")]
        public async Task<IActionResult> DeleteGame([FromBody] AddGameInput info)
        {
            return Ok();
        }

        [HttpGet("getGameList")]
        public async Task<ActionResult<List<TwitchAction>>> GetGameList()
        {
            var t = await _twitch.GetGamesFromList();
            return Ok(t);
        }
    }
}
