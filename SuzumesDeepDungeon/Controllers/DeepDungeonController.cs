using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore;
using SuzumesDeepDungeon.Data;
using SuzumesDeepDungeon.DTO;
using SuzumesDeepDungeon.Extensions;
using SuzumesDeepDungeon.Models;
using SuzumesDeepDungeon.Services;
using SuzumesDeepDungeon.Services.Rawg_Data;

namespace SuzumesDeepDungeon.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DeepDungeon : ControllerBase
{
    private readonly ILogger<DeepDungeon> _logger;
    private readonly DatabaseContext _context;

    public DeepDungeon(ILogger<DeepDungeon> logger, DatabaseContext context)
    {
        _logger = logger;
        _context = context;
    }
    
    [HttpGet(Name = "GetGameRank")]
    public async Task<ActionResult<IEnumerable<GameRankDTO>>> GetGameRank(
    [FromQuery] string? status,      
    [FromQuery] int? minRate,        
    [FromQuery] int? maxRate,        
    [FromQuery] string? name,
    [FromQuery] string? tags,        
    [FromQuery] string? sortBy,
    [FromQuery] DateTime? created,
    [FromQuery] DateTime? updated,
    [FromQuery] bool desc = false)   
    {

        var query = _context.GameRanks
            .Include(x => x.Stores)
            .Include(p => p.Screenshots)
            .Include(f => f.Trailers)
            .Include(t => t.Achievements)
            .Include(u => u.User)
            .Include(h => h.Tags)
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

        var gameRanks = await query.ToListAsync();
        var result = gameRanks.Select(x => x.GetDTO());

        return Ok(result);
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
            Created = DateTime.Now,
            Updated = DateTime.Now

        }).ToList() ?? new List<Stores>();


        var gameRank = new GameRank
        {
            Name = newGameRank.Name,
            Rate = newGameRank.Rate ?? 0,
            Status = (newGameRank.Status != null ? (GameStatus)Enum.Parse(typeof(GameStatus), newGameRank.Status) : GameStatus.Unknown),
            GameTime = TimeSpanExtensions.DecimalHoursToTimeSpan(newGameRank.GameTime ?? 0.0),
            Review = newGameRank.Review ?? "",
            Created = DateTime.Now,
            Updated = DateTime.Now,
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
                Created = DateTime.Now,
                Updated = DateTime.Now
            } : null,

            Tags = newGameRank.Tags?.Select(t => new GameTag
            {
                TagId = (int)t.TagId,
                Name = t.Name,
                Slug = t.Slug,
                Language = t.Language,
                GamesCount = (int)t.GamesCount,
                ImageBackground = t.ImageBackground,
                Created = DateTime.Now,
                Updated = DateTime.Now

            }).ToList() ?? new List<GameTag>(),
            Achievements = newGameRank.Achievements?.Select(a => new GameAchievement
            {
                Name = a.Name,
                Description = a.Description,
                ImageUrl = a.ImageUrl,
                CompletionPercent = a.CompletionPercent,
                Created = DateTime.Now,
                Updated = DateTime.Now
            }).ToList() ?? new List<GameAchievement>(),

            Trailers = newGameRank.Trailers?.Select(t => new Trailer
            {
                Name = t.Name,
                PreviewImageUrl = t.PreviewImageUrl,
                Video480p = t.Video480p,
                VideoMaxQuality = t.VideoMaxQuality,
                Created = DateTime.Now,
                Updated = DateTime.Now
            }).ToList() ?? new List<Trailer>(),
        };

        _context.GameRanks.Add(gameRank);
        await _context.SaveChangesAsync();

        _logger.LogInformation($"GameRank with name {newGameRank.Name} added successfully with ID {gameRank.Id}.");
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
        existingGameRank.Updated = DateTime.Now;
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
                    Created = DateTime.Now,
                    Updated = DateTime.Now
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
                    Created = DateTime.Now,
                    Updated = DateTime.Now
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
                    Created = DateTime.Now,
                    Updated = DateTime.Now
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
                    Created = DateTime.Now,
                    Updated = DateTime.Now
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


    

}
