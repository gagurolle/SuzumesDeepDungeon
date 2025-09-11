using SuzumesDeepDungeon.DTO;
using SuzumesDeepDungeon.Models;
using SuzumesDeepDungeon.Models.Minecraft;
using SuzumesDeepDungeon.Services.Rawg_Data;

namespace SuzumesDeepDungeon.Extensions
{
    public static class GetDto
    {
        public static GameRankDTO GetDTO(this GameRank gameRank)
        {
            return new GameRankDTO
            {
                Id = gameRank.Id,
                Name = gameRank.Name,
                Rate = Convert.ToInt32(gameRank.Rate),
                Status = gameRank.Status.ToString(),
                GameTime = gameRank.GameTime.ToDecimalHours(),
                Review = gameRank?.Review,
                User = gameRank?.User?.GetDTO(),
                Image = gameRank?.Image,
                YoutubeLink = gameRank?.YoutubeLink,
                MetacriticRate = gameRank?.MetacriticRate,
                Released = gameRank?.Released,
                Stores = gameRank?.Stores.Select(x => x.GetDTO()).ToList(),
                Screenshots = gameRank?.Screenshots?.GetDTO(),
                Achievements = gameRank?.Achievements.Select(x => x.GetDTO()).ToList(),
                Trailers = gameRank?.Trailers.Select(x => x.GetDTO()).ToList(),
                Tags = gameRank?.Tags?.Select(x => x.GetDTO()).ToList(),
                Created = gameRank?.Created,
                Updated = gameRank?.Updated,
                RawgId = gameRank?.RawgId
            };
        }
        public static ScreenshotDTO GetDTO(this Screenshot Screenshot)
        {
            return new ScreenshotDTO
            {
               Id = Screenshot.Id,
                GameId = Screenshot.GameId,
                SteamHeaderUrl = Screenshot.SteamHeaderUrl,
                SteamCapsuleUrl = Screenshot.SteamCapsuleUrl,
                Steam600x900Url = Screenshot.Steam600x900Url,   
                RawgBackgroundUrl = Screenshot.RawgBackgroundUrl,
                Created = Screenshot.Created,
                Updated = Screenshot.Updated


            };
        }
        public static TagDTO GetDTO(this GameTag tag)
        {
            return new TagDTO
            {
               Id = tag.Id,
                GameId = tag.GameId,
                TagId = tag.TagId,
                Name = tag.Name,
                Slug = tag.Slug,
                Language = tag.Language,
                GamesCount = tag.GamesCount,
                ImageBackground = tag.ImageBackground,
                Created = tag.Created,
                Updated = tag.Updated
            };
        }
        public static TrailerDTO GetDTO(this Trailer trailer)
        {
            return new TrailerDTO
            {
               Id = trailer.Id,
                GameId = trailer.GameId,
                Name = trailer.Name,
                PreviewImageUrl = trailer.PreviewImageUrl,
                Video480p = trailer.Video480p,
                VideoMaxQuality = trailer.VideoMaxQuality,
                Created = trailer.Created,
                Updated = trailer.Updated
            };
        }
        public static StoreDTO GetDTO(this Stores store)
        {
            return new StoreDTO
            {
                Id = store.Id,
                GameId = store.GameId,
                RawgId = store.RawgId,
                StoreId = store.StoreId,
                Url = store.Url,
                Created = store.Created,
                Updated = store.Updated


            };
        }

        public static AchievementDTO GetDTO(this GameAchievement achievement)
        {
            return new AchievementDTO
            {
                Id = achievement.Id,
                GameId = achievement.GameId,
                Name = achievement.Name,
                Description = achievement.Description,
                ImageUrl = achievement.ImageUrl,
                CompletionPercent = achievement.CompletionPercent,
                Created = achievement.Created,
                Updated = achievement.Updated
            };
        }

        public static UserDTO GetDTO(this User user)
        {
            return new UserDTO
            {
                Username = user.Username,
                IsAdmin = user.IsAdmin
            };
        }

        public static ExternalApiDTO GetDTO(this ExternalApi api)
        {
            return new ExternalApiDTO
            {
                Id = api.Id,
                Name = api.Name,
                Description = api.Description,
                UserId = api.UserId,
                Created = api.Created,
                IsActive = api.IsActive,
            };
        }

        public static MinecraftContentDTO GetDTO(this MinecraftContent content)
        {
            return new MinecraftContentDTO
            {
                Id = content.Id,
                Content = content.Content,
                Header = content.Header,
                Created = content.Created,
                Updated = content.Updated,
                User = content.User.GetDTO(),
            };
        }
        
        public static MinecraftMainContentDTO GetDTO(this MinecraftMainContent content)
        {
            return new MinecraftMainContentDTO
            {
                Id = content.Id,
                Header = content.Header,
                HeaderInfo = content.HeaderInfo,
                Adres  = content.Adres,
                Version = content.Version,
                Mod = content.Mod,
                Created = content.Created,
                Updated = content.Updated,
                User = content.User.GetDTO(),
            };
        }
    }
}
