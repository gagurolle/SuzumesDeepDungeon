using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using SuzumesDeepDungeon.Data;
using SuzumesDeepDungeon.DTO;
using SuzumesDeepDungeon.Extensions;
using SuzumesDeepDungeon.HelpClasses;
using SuzumesDeepDungeon.Models;
using SuzumesDeepDungeon.Services;
using SuzumesDeepDungeon.Services.Rawg_Data;
using System.Text.Json;

namespace SuzumesDeepDungeon.Controllers;
[ApiController]
[Route("api/[controller]")]
public class DeepDungeon : ControllerBase
{
    private readonly ILogger<DeepDungeon> _logger;
    private readonly DatabaseContext _context;
    private readonly IConnectionMultiplexer _redis;
    private readonly IDatabase _cache;
    private readonly TimeSpan _cacheExpiry = TimeSpan.FromHours(24);

    public DeepDungeon(ILogger<DeepDungeon> logger, DatabaseContext context, IConnectionMultiplexer redis)
    {
        _logger = logger;
        _context = context;
        _redis = redis;
        _cache = _redis.GetDatabase();

    }

    [HttpGet("GetGameRank")]
    public async Task<ActionResult<GameRankDTO>> GetGameRank(
    [FromQuery] int Id)
    {
        var cacheKey = $"GameRank:{Id}";
        try
        {

            string cachedData = await _cache.StringGetAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedData))
            {
                _logger.LogInformation($"returned cached data for gameRank {Id}");
                return Ok(JsonSerializer.Deserialize<GameRankDTO>(cachedData));
            }
        }
        catch(Exception e)
        {
            _logger.LogWarning("Something went wrong with Redis Caching - " + e.Message);
        }

        var query = await _context.GameRanks
            .Include(x => x.Stores)
            .Include(p => p.Screenshots)
            .Include(f => f.Trailers)
            .Include(t => t.Achievements)
            .Include(u => u.User)
            .Include(h => h.Tags).Where(x => x.Id == Id).FirstOrDefaultAsync();

        if(query == null)
        {
            return NotFound();
        }
        var result = query.GetDTO();

        string jsonResult = JsonSerializer.Serialize(result);

        // 3. Сохраняем результат запроса в кеш
        await _cache.StringSetAsync(cacheKey, jsonResult, _cacheExpiry);

        return Ok(result);
    }

    
    [HttpGet(Name = "GetGameRanks")]
    public async Task<ActionResult<PagedResponse<List<GameRankDTO>>>> GetGameRanks(
    [FromQuery] string? status = null,      
    [FromQuery] int? minRate = null,        
    [FromQuery] int? maxRate = null,        
    [FromQuery] string? name = null,
    [FromQuery] string? tags = null,        
    [FromQuery] string? sortBy = null,
    [FromQuery] DateTime? created = null,
    [FromQuery] DateTime? updated = null,
    [FromQuery] bool desc = false,
    [FromQuery] int page = 1,               
    [FromQuery] int pageSize = 30)
    {

        var cacheKey = GenerateGameRanksCacheKey(status, minRate, maxRate, name, tags, sortBy, created, updated, desc, page, pageSize);
        try
        {
            

            string cachedData = await _cache.StringGetAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedData))
            {
                _logger.LogInformation($"Returning cached game ranks list for key: {cacheKey}");
                var optionsf = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = false,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };


                var cachedResponse = JsonSerializer.Deserialize<PagedResponse<GameRankDTO>>(cachedData, optionsf);
                return Ok(cachedData);
            }
        }
        catch(Exception e)
        {
            _logger.LogWarning("Something went wrong with Redis Caching - " + e.Message);
        }

        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 30;
        if (pageSize > 100) pageSize = 100;
        
        var query = _context.GameRanks
            .Include(x => x.Stores)
            .Include(u => u.User)
            //.Include(h => h.Tags)
            .AsQueryable();


        if (!string.IsNullOrEmpty(name))
        {

            var searchTerm = name.ToLower();
            query = query.Where(g => g.Name.ToLower().Contains(searchTerm));
        }

        if (!string.IsNullOrEmpty(status))
        {
            if (Enum.TryParse<GameStatus>(status, true, out var statusFilter))
            {
                var t = statusFilter;
                query = query.Where(g => g.Status == statusFilter);
            }
        }

        if (minRate.HasValue)
        {
            query = query.Where(g => g.Rate >= minRate.Value);
        }

        if (maxRate.HasValue)
        {
            query = query.Where(g => g.Rate <= maxRate.Value);
        }

        if (created.HasValue)
        {
            query = query.Where(g => g.Created >= created.Value);
        }

        if (!string.IsNullOrEmpty(tags))
        {
            var tagList = tags.Split(',', StringSplitOptions.RemoveEmptyEntries);
            query = query.Where(g => g.Tags.Any(t => tagList.Contains(t.Name)));
        }

        query = (sortBy?.ToLower(), desc) switch
        {
            ("name", false) => query.OrderBy(g => g.Name),
            ("name", true) => query.OrderByDescending(g => g.Name),
            ("rate", false) => query.OrderBy(g => g.Rate),
            ("rate", true) => query.OrderByDescending(g => g.Rate),
            ("released", false) => query.OrderBy(g => g.Released),
            ("released", true) => query.OrderByDescending(g => g.Released),
            ("created", false) => query.OrderBy(g => g.Created),
            ("created", true) => query.OrderByDescending(g => g.Created),
            _ => query.OrderByDescending(g => g.Created) 
        };
            
        var totalCount = await query.CountAsync();


        var withoutPaging = await query.ToListAsync();

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var result = items.Select(x => x.GetDTO()).ToList();
        
        var response = new PagedResponse<GameRankDTO>
        {
            Items = result,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        };

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = false,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var jsonResponse = JsonSerializer.Serialize(response, options);
        await _cache.StringSetAsync(cacheKey, jsonResponse, _cacheExpiry);

        _logger.LogInformation($"Game ranks list cached with key: {cacheKey}");


        return Ok(response);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGameRank(int id)
    {
        var gameRank = await _context.GameRanks.FindAsync(id);
        if (gameRank == null)
        {
            _logger.LogWarning($"GameRank with ID {id} not found.");
            return NotFound();
        }

        _context.GameRanks.Remove(gameRank);
        await _context.SaveChangesAsync();

        await InvalidateGameRankCacheAsync(id);
        _logger.LogInformation($"GameRank с ID {id} удален. Соответствующий кеш сброшен.");

        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpPost(Name = "AddGameRank")]
    public async Task<ActionResult<GameRankDTO>> AddGameRank([FromBody] GameRankDTO newGameRank)
    {
        if (newGameRank == null || string.IsNullOrEmpty(newGameRank.Name))
        {
            _logger.LogError("Invalid GameRank data provided.");
            return BadRequest();
        }

        var existingGameRank = await _context.GameRanks
     .FirstOrDefaultAsync(g => g.Name.ToLower() == newGameRank.Name.ToLower());

        if (existingGameRank != null)
        {
            _logger.LogWarning($"GameRank with name {newGameRank.Name} already exists.");
            return Conflict();
        }
        if(newGameRank.Image == null || newGameRank.Image == "")
        {
            newGameRank.Image = "default.png";
        }


        var User = _context.Users.FirstOrDefaultAsync(x => x.Username == newGameRank.User.Username).Result;
        if (User == null)
        {
            _logger.LogWarning("User not found. Please provide a valid user.");
            return BadRequest("User not found. Please provide a valid user.");
        }

        var stores = newGameRank.Stores?.Select(s => new Stores
        {
            RawgId = s.RawgId,
            StoreId = (StoresEnum)s.StoreId,
            Url = s.Url,
            Created = DateTime.UtcNow,
            Updated = DateTime.UtcNow

        }).ToList() ?? new List<Stores>();


        var gameRank = new GameRank
        {
            Name = newGameRank.Name,
            Rate = newGameRank.Rate ?? 0,
            Status = (newGameRank.Status != null ? (GameStatus)Enum.Parse(typeof(GameStatus), newGameRank.Status) : GameStatus.Unknown),
            GameTime = TimeSpanExtensions.DecimalHoursToTimeSpan(newGameRank.GameTime ?? 0.0),
            Review = newGameRank.Review ?? "",
            Created = DateTime.UtcNow,
            Updated = DateTime.UtcNow,
            User = User,
            Image = newGameRank.Image,
            YoutubeLink = newGameRank.YoutubeLink ?? "",
            MetacriticRate = newGameRank.MetacriticRate,
            Released = newGameRank.Released,
            RawgId = newGameRank?.RawgId ?? "",

            Stores = stores,

            Screenshots = newGameRank.Screenshots != null ? new Screenshot
            {
                SteamHeaderUrl = newGameRank.Screenshots?.SteamHeaderUrl,
                SteamCapsuleUrl = newGameRank.Screenshots?.SteamCapsuleUrl,
                Steam600x900Url = newGameRank.Screenshots?.Steam600x900Url,
                RawgBackgroundUrl = newGameRank.Screenshots?.RawgBackgroundUrl,
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow
            } : null,

            Tags = newGameRank.Tags?.Select(t => new GameTag
            {
                TagId = (int)t.TagId,
                Name = t.Name,
                Slug = t.Slug,
                Language = t.Language,
                GamesCount = (int)t.GamesCount,
                ImageBackground = t.ImageBackground,
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow

            }).ToList() ?? new List<GameTag>(),
            Achievements = newGameRank.Achievements?.Select(a => new GameAchievement
            {
                Name = a.Name,
                Description = a.Description,
                ImageUrl = a.ImageUrl,
                CompletionPercent = a.CompletionPercent,
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow
            }).ToList() ?? new List<GameAchievement>(),

            Trailers = newGameRank.Trailers?.Select(t => new Trailer
            {
                Name = t.Name,
                PreviewImageUrl = t.PreviewImageUrl,
                Video480p = t.Video480p,
                VideoMaxQuality = t.VideoMaxQuality,
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow
            }).ToList() ?? new List<Trailer>(),
        };

        _context.GameRanks.Add(gameRank);
        await _context.SaveChangesAsync();

        await InvalidateGameRanksCacheAsync();
        _logger.LogInformation($"GameRank with name {newGameRank.Name} added successfully with ID {gameRank.Id}. Cache invalidated.");

        return CreatedAtAction(nameof(GetGameRank), new { id = gameRank.Id }, gameRank.GetDTO());

    }
    [Authorize(Roles = "Admin")]
    [HttpPatch()]
    public async Task<ActionResult<GameRankDTO>> UpdateGameRank([FromBody] GameRankDTO updatedGameRank)
    {
        if (updatedGameRank == null)
        {
            _logger.LogError("Invalid GameRank data provided.");
            return BadRequest();
        }
        var id = updatedGameRank.Id;
        var existingGameRank = await _context.GameRanks.Where(p => p.Id == id).Include(x => x.Stores).Include(p => p.Screenshots).Include(f => f.Trailers).Include(t => t.Achievements).Include(u => u.User).Include(u => u.Tags).FirstOrDefaultAsync();
        if (existingGameRank == null)
        {
            _logger.LogWarning($"GameRank with ID {id} not found.");
            return NotFound();
        }

        existingGameRank.Name = updatedGameRank.Name ?? existingGameRank.Name;
        existingGameRank.Rate = updatedGameRank.Rate ?? existingGameRank.Rate;
        existingGameRank.Status = (updatedGameRank.Status != null ? (GameStatus)Enum.Parse(typeof(GameStatus), updatedGameRank.Status) : existingGameRank.Status);
        existingGameRank.GameTime = TimeSpanExtensions.DecimalHoursToTimeSpan(updatedGameRank.GameTime ?? 0.0);
        existingGameRank.Review = updatedGameRank.Review ?? existingGameRank.Review;
        existingGameRank.Updated = DateTime.UtcNow;
        existingGameRank.Image = updatedGameRank.Image ?? existingGameRank.Image;
        existingGameRank.YoutubeLink = updatedGameRank.YoutubeLink ?? "";
        existingGameRank.MetacriticRate = updatedGameRank.MetacriticRate;
        existingGameRank.Released = updatedGameRank.Released;

        if(existingGameRank.RawgId != updatedGameRank.RawgId)
        {
            _context.Trailers.RemoveRange(existingGameRank.Trailers);
            _context.Tag.RemoveRange(existingGameRank.Tags);
            _context.Achievements.RemoveRange(existingGameRank.Achievements);
            _context.Stores.RemoveRange(existingGameRank.Stores);

            await _context.SaveChangesAsync();
        }
        

        if (existingGameRank.Screenshots != null)
        {
            existingGameRank.Screenshots.RawgBackgroundUrl = updatedGameRank.Screenshots.RawgBackgroundUrl ?? existingGameRank.Screenshots.RawgBackgroundUrl;
            existingGameRank.Screenshots.SteamHeaderUrl = updatedGameRank.Screenshots.SteamHeaderUrl ?? existingGameRank.Screenshots.SteamHeaderUrl;
            existingGameRank.Screenshots.SteamCapsuleUrl = updatedGameRank.Screenshots.SteamCapsuleUrl ?? existingGameRank.Screenshots.SteamCapsuleUrl;
            existingGameRank.Screenshots.Steam600x900Url = updatedGameRank.Screenshots.Steam600x900Url ?? existingGameRank.Screenshots.Steam600x900Url;

        }

        if(existingGameRank.Stores.Count() != updatedGameRank?.Stores?.Count())
        {

            _context.Stores.RemoveRange(existingGameRank.Stores);

            foreach (var store in updatedGameRank.Stores)
            {
                existingGameRank.Stores.Add(new Stores
                {
                    RawgId = store.RawgId,
                    StoreId = (StoresEnum)store.StoreId,
                    Url = store.Url,
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow
                });
            }
        }
       
        if(existingGameRank.Tags.Count() != updatedGameRank?.Tags?.Count())
        {
            _context.Tag.RemoveRange(existingGameRank.Tags);
            foreach (var tag in updatedGameRank?.Tags)
            {
                existingGameRank.Tags.Add(new GameTag
                {
                    TagId = (int)tag.TagId,
                    Name = tag.Name,
                    Slug = tag.Slug,
                    Language = tag.Language,
                    GamesCount = (int)tag.GamesCount,
                    ImageBackground = tag.ImageBackground,
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow
                });
            }
        }
        if(existingGameRank.Achievements.Count() != updatedGameRank?.Achievements?.Count())
        {
            _context.Achievements.RemoveRange(existingGameRank.Achievements);
            foreach (var achievement in updatedGameRank?.Achievements)
            {
                existingGameRank.Achievements.Add(new GameAchievement
                {
                    Name = achievement.Name,
                    Description = achievement.Description,
                    ImageUrl = achievement.ImageUrl,
                    CompletionPercent = achievement.CompletionPercent,
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow
                });
            }
        }
        if(existingGameRank.Trailers.Count() != updatedGameRank?.Trailers?.Count())
        {
            _context.Trailers.RemoveRange(existingGameRank.Trailers);
            foreach (var trailer in updatedGameRank?.Trailers)
            {
                existingGameRank.Trailers.Add(new Trailer
                {
                    Name = trailer.Name,
                    PreviewImageUrl = trailer.PreviewImageUrl,
                    Video480p = trailer.Video480p,
                    VideoMaxQuality = trailer.VideoMaxQuality,
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow
                });
            }
        }

        existingGameRank.RawgId = updatedGameRank.RawgId ?? existingGameRank.RawgId;

        try
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                _logger.LogInformation($"GameRank with ID {id} updated successfully.");

                await InvalidateGameRankCacheAsync(id);

                _logger.LogInformation($"GameRank with ID {id} updated successfully. Cache invalidated.");
            }
            catch
            {
                await transaction.RollbackAsync();
                throw new Exception("Couldnt Rollback Transaction");
            }
           
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.GameRanks.Any(e => e.Id == id))
            {
                return NotFound();
            }
            throw;
        }

        return Ok(existingGameRank.GetDTO());
    }


    private string GenerateGameRanksCacheKey(
    string? status, int? minRate, int? maxRate, string? name,
    string? tags, string? sortBy, DateTime? created, DateTime? updated,
    bool desc, int page, int pageSize)
    {
        var keyParts = new List<string> { "GameRanks" };

        if (!string.IsNullOrEmpty(status)) keyParts.Add($"status:{status}");
        if (minRate.HasValue) keyParts.Add($"minRate:{minRate}");
        if (maxRate.HasValue) keyParts.Add($"maxRate:{maxRate}");
        if (!string.IsNullOrEmpty(name)) keyParts.Add($"name:{name.ToLower()}");
        if (!string.IsNullOrEmpty(tags)) keyParts.Add($"tags:{tags.ToLower()}");
        if (!string.IsNullOrEmpty(sortBy)) keyParts.Add($"sortBy:{sortBy.ToLower()}");
        if (created.HasValue) keyParts.Add($"created:{created:yyyy-MM-dd}");
        if (updated.HasValue) keyParts.Add($"updated:{updated:yyyy-MM-dd}");
        keyParts.Add($"desc:{desc}");
        keyParts.Add($"page:{page}");
        keyParts.Add($"pageSize:{pageSize}");

        return string.Join("|", keyParts);
    }

    // Метод для инвалидации всех кешей списков игр (вызывается в POST, PATCH, DELETE)
    private async Task InvalidateGameRanksCacheAsync()
    {
        var server = _redis.GetServer(_redis.GetEndPoints().First());
        var keys = server.Keys(pattern: "GameRanks*").ToArray();

        if (keys.Any())
        {
            await _cache.KeyDeleteAsync(keys);
            _logger.LogInformation($"Invalidated {keys.Length} game ranks cache entries");
        }
    }



    private async Task InvalidateGameRankCacheAsync(int gameRankId)
    {
        var cacheKey = $"GameRank:{gameRankId}";
        await _cache.KeyDeleteAsync(cacheKey);
        await InvalidateGameRanksCacheAsync();
    }

}
