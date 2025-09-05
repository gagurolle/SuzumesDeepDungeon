using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SuzumesDeepDungeon.Attributes;
using SuzumesDeepDungeon.Data;
using SuzumesDeepDungeon.DTO;
using SuzumesDeepDungeon.Extensions;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using static SuzumesDeepDungeon.Models.User;

namespace SuzumesDeepDungeon.Controllers
{

    [ApiController]
    [Route("api/ext")]
    public class ExternalApiController : ControllerBase
    {

        private readonly IConfiguration _config;
        private readonly DatabaseContext _context;

        public ExternalApiController(IConfiguration config, DatabaseContext context)
        {
            _config = config;
            _context = context;
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost(Name = "AddApiKey")]
        public async Task<ActionResult<ExternalApiDTO>> AddApiKey([FromBody] ExternalApiDTO api)
        {

            if (api.User == null && api.UserId == null) {
                return BadRequest("User information is required");
            }


            bool keyExist = true;
            string key = string.Empty;
            string hashKey = string.Empty;
            ///create api 
            while (keyExist)
            {
                key = GenerateKey();
                hashKey = HashKey(key);


                if (await _context.Api.Where(x => x.HashKey == hashKey).FirstOrDefaultAsync() != null)
                {
                    continue;
                }
                keyExist = false;
            }

            var User = api.User != null ? await _context.Users.Where(x => x.Username == api.User.Username).FirstOrDefaultAsync() : await _context.Users.Where(x => x.Id == api.UserId).FirstOrDefaultAsync();

            _context.Api.Add(new Models.ExternalApi
            {
                HashKey = hashKey,
                Name = api.Name ?? $"key:{DateTime.Now} | {User.Username} | {() => { var t = new Random().Next(); return t.ToString(); } }",
                Description = api.Description ?? null,
                UserId = User.Id,
                Created = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();
            api.Key = key;
            return Ok(api);
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet(Name = "GetApiKeys")]
        public async Task<ActionResult<List<ExternalApiDTO>>> GetApiKeys([FromQuery] string username)
        {

            var keys = await _context.Api.Where(x => x.User.Username == username).Select(g => g.GetDTO()).ToListAsync();
            if(keys == null)
            {
                return NotFound("No API keys found for the user");
            }
            return Ok(keys);
        }


        //[Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKey(int id)
        {
            var apiKey =await _context.Api.Where(x => x.Id == id).FirstOrDefaultAsync();
            if(apiKey == null)
            {
                return NotFound("API key not found");
            }
                _context.Api.Remove(apiKey);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("VerifyKey")]
        public async Task<IActionResult> VerifyKey([FromBody] string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return BadRequest("API key is required");
            }

            var apiKeys = await _context.Api
                .Select(k => k.HashKey)
                .ToListAsync();

            var isValid = apiKeys.Any(hash => VerifyKey(key, hash));

            if (!isValid)
            {
                return NotFound("Invalid API key");
            }

            return Ok("API key is valid");
        }

        [ApiKeyAuth]
        [HttpGet("TestAuth")]
        public IActionResult TestAuth()
        {
            return Ok("You have access to this protected resource.");
        }


        public static string GenerateKey()
        {
            byte[] key = new byte[16];
            using var generator = RandomNumberGenerator.Create();
            generator.GetBytes(key);
            return Convert.ToBase64String(key);
        }
        public static string HashKey(string key)
        {
            return BCrypt.Net.BCrypt.EnhancedHashPassword(key, 13);
        }

        public static bool VerifyKey(string key, string hash)
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(key, hash);
        }


    }
}