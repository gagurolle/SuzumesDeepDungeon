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
using SuzumesDeepDungeon.Extensions;
using SuzumesDeepDungeon.HelpClasses;
using SuzumesDeepDungeon.Models.Minecraft;

namespace SuzumesDeepDungeon.Controllers
{
    [Route("api/minecraft")]
    [ApiController]
    public class MinecraftController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly DatabaseContext _context;
        private readonly ILogger<DeepDungeon> _logger;

        public MinecraftController(ILogger<DeepDungeon> logger, IConfiguration config, DatabaseContext context, TwitchService twitch)
        {
            _config = config;
            _context = context;
            _logger = logger;
        }


        [HttpGet("GetContents")]
        public async Task<ActionResult<List<MinecraftContentDTO>>> GetMinecraftContents( [FromQuery] int page = 1,               
            [FromQuery] int pageSize = 30)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 30;
            if (pageSize > 100) pageSize = 100;
            
            
            var query = _context.MinecraftContents
                .Include(u => u.User)
                .AsQueryable();
            
            var totalCount = await query.CountAsync();
            var withoutPaging = await query.ToListAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            
            var result = items.Select(x => x.GetDTO());
            
            var response = new PagedResponse<MinecraftContentDTO>
            {
                Items = result,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };

            return Ok(response);
        }

        [HttpPost("NewContent")]
        public async Task<IActionResult> InsertNewMinecraftContent([FromBody] MinecraftContentDTO minecraftContent)
        {

            if (minecraftContent.User == null || minecraftContent.User.Username == null)
            {
                return Unauthorized("No user in request.");
            }
            var user = await _context.Users.Where(x => x.Username == minecraftContent.User.Username).FirstOrDefaultAsync();
            if (user == null)
            {
                return Unauthorized("No valid user found.");
            }
            MinecraftContent newMinecraftContent = new MinecraftContent()
            {
                Content = minecraftContent.Content,
                Header = minecraftContent.Header,
                Created = DateTime.Now,
                Updated = DateTime.Now,
                UserId = user.Id
            };
            _context.MinecraftContents.Add(newMinecraftContent);
            await _context.SaveChangesAsync();
            return Ok(newMinecraftContent);
        }

        [HttpPatch("UpdateContent")]
        public async Task<IActionResult> UpdateContent([FromBody] MinecraftContentDTO minecraftContent)
        {
            var item = await _context.MinecraftContents.Where(x => x.Id == minecraftContent.Id).FirstOrDefaultAsync();
            
            item.Content = minecraftContent.Content;
            item.Header = minecraftContent.Header;
            item.Updated = DateTime.Now;
            
            await _context.SaveChangesAsync();
            return Ok(item);
        }

        [HttpDelete("DeleteContent")]
        public async Task<IActionResult> DeleteContent(int id)
        {
            
            _context.MinecraftContents.Remove(new MinecraftContent() { Id = id });
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("UpdateMainContent")]
        public async Task<ActionResult<MinecraftMainContent>> EditMainContent([FromBody] MinecraftMainContentDTO minecraftContent)
        {
            if (minecraftContent.User == null || minecraftContent.User.Username == null)
            {
                return Unauthorized("No user in request.");
            }
            var user = await _context.Users.Where(x => x.Username == minecraftContent.User.Username).FirstOrDefaultAsync();
            if (user == null)
            {
                return Unauthorized("No valid user found.");
            }
            var t = await _context.MinecraftMainContents.FirstOrDefaultAsync();
            if (t == null)
            {
                
                t = new MinecraftMainContent()
                {
                    Created = DateTime.Now,
                    Updated = DateTime.Now,
                    Header = minecraftContent.Header,
                    HeaderInfo = minecraftContent.HeaderInfo,
                    Mod = minecraftContent.Mod,
                    Version = minecraftContent.Version,
                    Adres = minecraftContent.Adres,
                    UserId = user.Id
                };
                _context.MinecraftMainContents.Add(t);
                await _context.SaveChangesAsync();
            }
            else
            {
                t.Header = minecraftContent.Header;
                t.HeaderInfo = minecraftContent.HeaderInfo;
                t.Mod = minecraftContent.Mod;
                t.Version = minecraftContent.Version;
                t.Adres = minecraftContent.Adres;
                t.Updated = DateTime.Now;
                
                await _context.SaveChangesAsync();
            }
            return Ok(t);
        }

        [HttpGet("GetMainContent")]
        public async Task<ActionResult<MinecraftMainContentDTO>> GetMainContent()
        {
            var minecraftMainContent = await _context.MinecraftMainContents.FirstOrDefaultAsync();
            if (minecraftMainContent == null)
            {
                return NotFound();
            }
            return minecraftMainContent.GetDTO();
        }
    }
}
