using SuzumesDeepDungeon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuzumesDeepDungeon.Test.UnitTest
{
    internal class MockModel
    {
        public GameRank returnGamerank()
        {
            return new GameRank()
            {
                Achievements = new List<GameAchievement>()
                {
                    new GameAchievement()
                    {
                        Name = "Achievement 1",
                        Description = "Description 1",
                        ImageUrl = "Image1.png",
                        Created = DateTime.Now,
                        Updated = DateTime.Now
                    },
                    new GameAchievement()
                    {
                        Name = "Achievement 2",
                        Description = "Description 2",
                        ImageUrl = "Image2.png",
                        Created = DateTime.Now,
                        Updated = DateTime.Now
                    }
                },
                Created = DateTime.Now,
                GameTime = TimeSpan.FromHours(10),
                Image = "GameImage.png",
                MetacriticRate = 85,
                Name = "Test Game",
                Rate = 9.5,
                Id = 1,
                RawgId = "rawg-12345",
                Released = "2023-01-01",
                Review = "Great game!",
                Status = GameStatus.Completed,
                Tags = new List<GameTag>()
                {
                    new GameTag() { Name = "Action", ImageBackground ="", GameId=1, Language="", TagId=12, Created=DateTime.Now, Updated=DateTime.Now, Slug=""},
                    new GameTag() { Name = "Adventure", ImageBackground ="", GameId=1, Language="", TagId=13, Created=DateTime.Now, Updated=DateTime.Now, Slug = ""}
                },
                Trailers = new List<Trailer>()
                {
                    new Trailer()
                    {
                        Name = "Trailer 1",
                        PreviewImageUrl = "Preview1.png",
                        Video480p = "Video480p1.mp4",
                        VideoMaxQuality = "VideoMax1.mp4",
                        Created = DateTime.Now,
                        Updated = DateTime.Now
                    }
                },
                Updated = DateTime.Now,
                User = new User()
                {
                    Id = 1,
                    Username = "testuser",
                    Email = "",
                    Password= "Password",

                },
                Screenshots = new Screenshot()
                {
                    Id = 1,
                    RawgBackgroundUrl = "https://example.com/screenshot1.png",
                    Steam600x900Url = "",
                    SteamCapsuleUrl = "",
                    SteamHeaderUrl = "",
                    Created = DateTime.Now,
                    Updated = DateTime.Now
                },
                Stores = new List<Stores>()
                {
                    new Stores()
                    {
                        StoreId = StoresEnum.Steam,
                        Url = "https://store.steampowered.com/app/12345",
                        Created = DateTime.Now,
                        Updated = DateTime.Now,
                        RawgId = "1"
                    }
                },

            };
        }
    
        public List<GameRank> returnGameranks()
        {
            return new List<GameRank>()
            {
                new()
                {
                Created = DateTime.Now,
                GameTime = TimeSpan.FromHours(60),
                Image = "GameImageGGGh.png",
                MetacriticRate = 50,
                Name = "GoodGame1Test",
                Rate = 4,
                RawgId = "rawg-6543s",
                Released = "2021-02-03",
                Review = "bad gameF!",
                Status = GameStatus.Drop,
                Tags = new List<GameTag>()
                {
                    new GameTag() { Name = "JRPG", ImageBackground ="",  Language="", TagId=12, Created=DateTime.Now, Updated=DateTime.Now, Slug=""},
                    new GameTag() { Name = "Race", ImageBackground ="",  Language="", TagId=13, Created=DateTime.Now, Updated=DateTime.Now, Slug = ""}
                },
                    
                Stores = new List<Stores>()
                {
                    new Stores()
                    {
                        StoreId = StoresEnum.Steam,
                        Url = "https://store.steampowered.com/app/12345",
                        Created = DateTime.Now,
                        Updated = DateTime.Now,
                        RawgId = "rawg-6543s"
                    }
                },
                },

                new()
                {
                Created = DateTime.Now,
                GameTime = TimeSpan.FromHours(60),
                Image = "GameImage.png",
                MetacriticRate = 50,
                Name = "GoodGame3",
                Id=6,
                Rate = 7,
                RawgId = "rawg-6543",
                Released = "2021-02-03",
                Review = "bad game!",
                Status = GameStatus.Completed,
                Tags = new List<GameTag>()
                {
                    new GameTag() { Name = "Action", ImageBackground ="", Language="", TagId=12, Created=DateTime.Now, Updated=DateTime.Now, Slug=""},
                    new GameTag() { Name = "RPG", ImageBackground ="", Language="", TagId=13, Created=DateTime.Now, Updated=DateTime.Now, Slug = ""}
                },
                Stores = new List<Stores>()
                {
                    new Stores()
                    {
                        StoreId = StoresEnum.Steam,
                        Url = "https://store.steampowered.com/app/12345",
                        Created = DateTime.Now,
                        Updated = DateTime.Now,
                        RawgId = "rawg-6543s"
                    }
                },
                }

            };
        }

    }
}
