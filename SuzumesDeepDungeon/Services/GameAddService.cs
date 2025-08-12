using Microsoft.AspNetCore.Mvc;
using SuzumesDeepDungeon.Models;
using SuzumesDeepDungeon.Services.Rawg_Data;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace SuzumesDeepDungeon.Services
{


    /*Сделать сервис для поиска игр на стороннем сервисе (rawg.io, steam API)
     * Мне нужно получать одинаковые картинки, 
     * отзыв с metacritic, 
     * ссылку на steam, 
     * стоимость игры в рублях и долларах (будет два поля, первичная стоимость и текущая, которое будет меняться каждый раз при обновлении страницы)
     * Сколько заняло времени на прохождение игры в среднем у людей (howlongtobeat)
     * 
     * сsv загрузка игр в базу данных с twitch statistc
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     */

    public static class GameAddService
    {

        public static string rawgApi { get; set; } = ""; // rawg, steam
        public static class SteamApi
        {
            public const string SteamImageBaseUrl_header = "https://cdn.akamai.steamstatic.com/steam/apps/{0}/header.jpg";
            public const string SteamImageBaseUrl_capsule = "https://cdn.akamai.steamstatic.com/steam/apps/{0}/capsule_184x69.jpg";
            public const string SteamImageBaseUrl_600x900 = "https://cdn.akamai.steamstatic.com/steam/apps/{0}/library_600x900.jpg";

            public static async Task<string> GetGameJpg(string steamUrl)
            {
                var id = GetSteamAppIdFromUrl(steamUrl);

                var t = await GetGameCoverAsync(id);

                return t;

            }


            public static async Task<string> GetGameCoverAsync(string steamAppId, string url = SteamImageBaseUrl_600x900)
            {

                if(steamAppId == "")
                {
                    Console.WriteLine("Empty steam Id game");
                    return null;
                }
                var imageUrl = string.Format(url, steamAppId);

                // Проверяем, существует ли изображение
                using var httpClient = new HttpClient();
                var response = await httpClient.GetAsync(imageUrl, HttpCompletionOption.ResponseHeadersRead);

                if (response.IsSuccessStatusCode)
                    return imageUrl;

                return null; // Если обложка не найдена
            }

            public static string GetSteamAppIdFromUrl(string url)
            {
                string pattern = @"store\.steampowered\.com/app/(\d+)";
                var match = Regex.Match(url, pattern);

                if (match.Success && match.Groups.Count > 1)
                {
                    return match.Groups[1].Value;
                }
                else
                {

                    string pattern2 = @"store\.steampowered\.com/sub/(\d+)";
                    var match2 = Regex.Match(url, pattern2);

                    if (match2.Success && match2.Groups.Count > 1)
                    {
                        return match2.Groups[1].Value;
                    }
                    Console.WriteLine("Invalid url");
                    return "";
                }
                
            }

        }
    


        public static class RawgAPI
        {
            public static async Task<RawgGameCompleteData> GetGameFullInfo(string? gameName = null, string? id = null)
            {
                if(gameName == null && id == null)
                {
                    throw new ArgumentNullException(nameof(gameName) + nameof(id));
                }

                if (id == null)
                {
                    var games = FindGames(gameName).Result.ToList();
                    id = games?.FirstOrDefault()?.Id.ToString();
                }

                var game = GetGameDetail(id);
                var trailers = GetGameTrailers(id);
                var stores = GetGameStores(id);
                var achievements = GetGameAchievements(id);
                var steamUrl = "";

                if (stores?.Results != null)
                {
                    var steam = stores.Results.Where(p => p.StoreId == 1).FirstOrDefault();
                    if (steam != null)
                    {
                        steamUrl = steam.Url;
                    }
                }

                return new RawgGameCompleteData
                {
                    game = game,
                    trailers = trailers,
                    stores = stores,
                    achievements = achievements,
                    steamUrl = steamUrl
                };

               
            }

            public static async Task<GameRank> RawgDataToGameRank(RawgGameCompleteData data, User user = null)
            {
                var gameRank = new GameRank()
                {
                    Name = data.game.Name,
                    Rate = 0,
                    Status = GameStatus.Unknown,
                    GameTime = TimeSpan.Zero, // Время прохождения игры
                    Review = "",
                    User = user,
                    MetacriticRate = data?.game?.Metacritic,
                    Released = data?.game?.Released,
                    Created = DateTime.Now,
                    Updated = DateTime.Now,
                    RawgId = data?.game?.Id.ToString() ?? string.Empty,
                };


                var trailers = new List<Trailer>();
                foreach (var tr in data?.trailers?.Results)
                {
                    trailers.Add(new Trailer()
                    {
                        Name = data.game.Name,
                        PreviewImageUrl = tr.PreviewImageUrl,
                        Video480p = tr.Data.Video480p,
                        VideoMaxQuality = tr.Data.VideoMaxQuality,
                        Created = DateTime.Now,
                        Updated = DateTime.Now
                    });
                }
                var stores = new List<Stores>();
                foreach (var store in data.stores?.Results)
                {
                    stores.Add(new Stores()
                    {
                        RawgId = data.game.Id.ToString(),
                        StoreId = (StoresEnum)store.StoreId,
                        Url = store.Url,
                        Created = DateTime.Now,
                        Updated = DateTime.Now
                    });
                }
                var achievements = new List<GameAchievement>();
                foreach (var ach in data.achievements?.Results)
                {
                    achievements.Add(new GameAchievement()
                    {
                        Name = ach.Name,
                        Description = ach.Description,
                        ImageUrl = ach.ImageUrl,
                        CompletionPercent = ach.CompletionPercent,
                        Created = DateTime.Now,
                        Updated = DateTime.Now
                    });
                }

                var tags = new List<GameTag>();
                foreach (var tag in data.game.Tags)
                {
                    tags.Add(new GameTag()
                    {
                        Name = tag.Name,
                        TagId = tag.Id,
                        Slug = tag.Slug,
                        Language = tag.Language,
                        GamesCount = tag.GamesCount,
                        ImageBackground = tag.ImageBackground,
                        Created = DateTime.Now,
                        Updated = DateTime.Now

                    });
                }

               

                gameRank.Trailers = trailers;
                gameRank.Stores = stores;
                gameRank.Achievements = achievements;
                gameRank.Tags = tags;

                if (data?.steamUrl != null)
                {
                    try
                    {

                        var steamIdFromurl = SteamApi.GetSteamAppIdFromUrl(data?.steamUrl);

                        gameRank.Screenshots = new Screenshot()
                        {
                            SteamHeaderUrl = await SteamApi.GetGameCoverAsync(steamIdFromurl, SteamApi.SteamImageBaseUrl_header),
                            SteamCapsuleUrl = await SteamApi.GetGameCoverAsync(steamIdFromurl, SteamApi.SteamImageBaseUrl_capsule),
                            Steam600x900Url = await SteamApi.GetGameCoverAsync(steamIdFromurl, SteamApi.SteamImageBaseUrl_600x900),
                            RawgBackgroundUrl = data.game.BackgroundImage,
                            Created = DateTime.Now,
                            Updated = DateTime.Now
                        };
                        gameRank.Image = gameRank.Screenshots.Steam600x900Url;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Чет пошло не так в стиме");
                        gameRank.Image = gameRank.Screenshots.RawgBackgroundUrl;
                    }
                }
                else {
                    gameRank.Image = gameRank.Screenshots.RawgBackgroundUrl;
                
                }
                    return gameRank;
            }


            public static GameVideosResponse GetGameTrailers(string id)
            {
                using (var client = new HttpClient())
                {
                    string urlGameDetail = $"https://api.rawg.io/api/games/{id}/movies?key={rawgApi}";
                    var response = client.GetAsync(urlGameDetail).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var contentDetail = response.Content.ReadAsStringAsync().Result;
                        try
                        {
                            var options = new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true, // Игнорировать регистр свойств
                                AllowTrailingCommas = true,        // Разрешать запятые в конце
                                ReadCommentHandling = JsonCommentHandling.Skip // Пропускать комментарии
                            };

                            var content = JsonSerializer.Deserialize<GameVideosResponse>(contentDetail, options);

                            return content;
                        }

                        catch (JsonException ex)
                        {
                            Console.WriteLine($"Ошибка десериализации: {ex.Message}");
                            return null;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Ошибка: {ex.Message}");
                            return null;
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Ошибка: {response.StatusCode}");
                        return null;
                    }
                }
            }
            public static StoreApiResponse GetGameStores(string id)
            {
                using (var client = new HttpClient())
                {
                    string urlGameDetail = $"https://api.rawg.io/api/games/{id}/stores?key={rawgApi}";
                    var response = client.GetAsync(urlGameDetail).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var contentDetail = response.Content.ReadAsStringAsync().Result;
                        try
                        {
                            var options = new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true, // Игнорировать регистр свойств
                                AllowTrailingCommas = true,        // Разрешать запятые в конце
                                ReadCommentHandling = JsonCommentHandling.Skip // Пропускать комментарии
                            };

                            var content = JsonSerializer.Deserialize<StoreApiResponse>(contentDetail, options);

                            return content;
                        }

                        catch (JsonException ex)
                        {
                            Console.WriteLine($"Ошибка десериализации: {ex.Message}");
                            return null;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Ошибка: {ex.Message}");
                            return null;
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Ошибка: {response.StatusCode}");
                        return null;
                    }
                }
            }
            public static AchievementsApiResponse GetGameAchievements(string id)
            {
                using (var client = new HttpClient())
                {
                    string urlGameDetail = $"https://api.rawg.io/api/games/{id}/achievements?key={rawgApi}";
                    var responseDetail = client.GetAsync(urlGameDetail).Result;

                    if (responseDetail.IsSuccessStatusCode)
                    {
                        var contentDetail = responseDetail.Content.ReadAsStringAsync().Result;
                        try
                        {
                            var options = new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true, // Игнорировать регистр свойств
                                AllowTrailingCommas = true,        // Разрешать запятые в конце
                                ReadCommentHandling = JsonCommentHandling.Skip // Пропускать комментарии
                            };

                            var content = JsonSerializer.Deserialize<AchievementsApiResponse>(contentDetail, options);

                            return content;
                        }


                        catch (JsonException ex)
                        {
                            Console.WriteLine($"Ошибка десериализации: {ex.Message}");
                            return null;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Ошибка: {ex.Message}");
                            return null;
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Ошибка: {responseDetail.StatusCode}");
                        return null;
                    }
                }
            }
            public static async Task<List<Game>> FindGames(string gameName)
            {
                string urlSearch = $"https://api.rawg.io/api/games?key={rawgApi}&page_size=10&search={Uri.EscapeDataString(gameName)}";
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(urlSearch);
                    if (response.IsSuccessStatusCode)
                    {
                        var content = response.Content.ReadAsStringAsync().Result;

                        var t = JsonNode.Parse(content);
                        var options = new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true, // Игнорировать регистр свойств
                            AllowTrailingCommas = true,        // Разрешать запятые в конце
                            ReadCommentHandling = JsonCommentHandling.Skip // Пропускать комментарии
                        };

                        try
                        {
                            // Десериализация JSON в объект
                            var responseJson = JsonSerializer.Deserialize<RawgApiResponse>(content, options);

                            if(responseJson == null)
                            {
                                Console.WriteLine($"0 games");
                                return new List<Game>();
                            }
                            return responseJson.Results.ToList();



                        }
                        catch (JsonException ex)
                        {
                            Console.WriteLine($"Ошибка десериализации: {ex.Message}");
                            return null;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Ошибка: {ex.Message}");
                            return null;
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Ошибка: {response.StatusCode}");
                        return null;
                    }


                }
            }
            public static Game GetGameDetail(string id)
            {
                using (var client = new HttpClient())
                {
                    string urlGameDetail = $"https://api.rawg.io/api/games/{id}?key={rawgApi}";
                    var responseDetail = client.GetAsync(urlGameDetail).Result;

                    if (responseDetail.IsSuccessStatusCode)
                    {
                        var contentDetail = responseDetail.Content.ReadAsStringAsync().Result;
                        try
                        {
                            var options = new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true, // Игнорировать регистр свойств
                                AllowTrailingCommas = true,        // Разрешать запятые в конце
                                ReadCommentHandling = JsonCommentHandling.Skip // Пропускать комментарии
                            };

                            var contentDetailJson = JsonSerializer.Deserialize<Game>(contentDetail, options);

                            return contentDetailJson;
                        }


                        catch (JsonException ex)
                        {
                            Console.WriteLine($"Ошибка десериализации: {ex.Message}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Ошибка: {ex.Message}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Ошибка: {responseDetail.StatusCode}");
                        return null;
                    }

                    return null;
                }
            }
        }
    }
}
