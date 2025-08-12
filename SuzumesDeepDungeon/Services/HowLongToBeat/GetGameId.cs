
using System.Text;
using System.Text.Json;

namespace SuzumesDeepDungeon.Services.HowLongToBeat
{


    public static class HowLongToBeatSeeker
    {
        private static readonly string[] UserAgents = {
        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/138.0.0.0 Safari/537.36",
        "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/17.0 Safari/605.1.15",
        "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:126.0) Gecko/20100101 Firefox/126.0"
    };

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        private static readonly Random _random = new();

        public static async Task<HltbSearchResult> SeekGame(string gameName, HttpClient httpClient, CancellationToken cancellationToken = default)
        {
            // Вычисляем CRC64 хеш
            string crc64Hash = Crc64.Compute(gameName.ToLower());

            // Формируем URL
            string url = $"https://howlongtobeat.com/api/seek/{crc64Hash}";

            // Формируем payload согласно новому формату
            var payload = new
            {
                searchType = "games",
                searchTerms = new[] { gameName }, // Теперь передаем массив с названием игры
                searchPage = 1,
                size = 20,
                searchOptions = new
                {
                    games = new
                    {
                        userId = 0,
                        platform = "",
                        sortCategory = "popular",
                        rangeCategory = "main",
                        rangeTime = new { min = (int?)null, max = (int?)null }, // null вместо 0
                        gameplay = new
                        {
                            perspective = "",
                            flow = "",
                            genre = "",
                            difficulty = "" // Добавлено новое поле
                        },
                        rangeYear = new { min = "", max = "" }, // Добавлен новый блок
                        modifier = ""
                    },
                    users = new { sortCategory = "postcount" },
                    lists = new { sortCategory = "follows" }, // Добавлен новый блок
                    filter = "",
                    sort = 0,
                    randomizer = 0
                },
                useCache = true // Добавлен новый параметр
            };

            var json = JsonSerializer.Serialize(payload, JsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = content
            };

            AddRequestHeaders(request);

            using var response = await httpClient.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync(cancellationToken);
            return JsonSerializer.Deserialize<HltbSearchResult>(responseJson, JsonOptions);
        }

        private static void AddRequestHeaders(HttpRequestMessage request)
        {
            var randomAgent = UserAgents[_random.Next(UserAgents.Length)];

            request.Headers.Add("User-Agent", randomAgent);
            request.Headers.Add("Origin", "https://howlongtobeat.com");
            request.Headers.Add("Referer", "https://howlongtobeat.com/");
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("Accept-Language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7");
            request.Headers.Add("Priority", "u=1, i");
            request.Headers.Add("Sec-Ch-Ua", "\"Not)A;Brand\";v=\"8\", \"Chromium\";v=\"138\", \"Google Chrome\";v=\"138\"");
            request.Headers.Add("Sec-Ch-Ua-Mobile", "?0");
            request.Headers.Add("Sec-Ch-Ua-Platform", "\"Windows\"");
            request.Headers.Add("Sec-Fetch-Dest", "empty");
            request.Headers.Add("Sec-Fetch-Mode", "cors");
            request.Headers.Add("Sec-Fetch-Site", "same-origin");
        }

    }

    public class HltbSearchResult
    {
        public int Count { get; set; }
        public List<HltbGame> Data { get; set; } = new List<HltbGame>();
        public int PageCurrent { get; set; }
        public int PageTotal { get; set; }
    }

    public class HltbGame
    {
        public int GameId { get; set; }
        public string GameName { get; set; }
        public string GameNameClear { get; set; }
        public string ProfilePlatform { get; set; }
        public string GameImage { get; set; }
        public int CompMain { get; set; }
        public int CompPlus { get; set; }
        public int Comp100 { get; set; }
        public int CompAll { get; set; }
        public int CompMainCount { get; set; }
        public int CompPlusCount { get; set; }
        public int Comp100Count { get; set; }
        public int CompAllCount { get; set; }
        public int InvestedCo { get; set; }
        public int SpeedRun { get; set; }
        public int SpeedRunCount { get; set; }
        public int ReviewScore { get; set; }
        public string GameWebLink { get; set; }
        public string ProfileDev { get; set; }
        public string ProfilePub { get; set; }
        public string ProfileGenre { get; set; }
        public int ReleaseWorld { get; set; }
        public string RatingRatio { get; set; }
        public int ProfileSteam { get; set; }
        public int ProfilePlatformId { get; set; }
        public int ProfileEshop { get; set; }
        public int ProfileXbox { get; set; }
        public int ProfilePsn { get; set; }
        public int ProfilePsnId { get; set; }
        public int ProfileXboxId { get; set; }
        public int ProfileEshopId { get; set; }
        public int ProfileItch { get; set; }
        public int ProfileItchId { get; set; }
        public int ProfileSteamId { get; set; }
        public int CountPlaying { get; set; }
        public int CountBacklog { get; set; }
        public int CountReplay { get; set; }
        public int CountRetired { get; set; }
        public string ReviewScoreWord { get; set; }
        public int CountCustom { get; set; }
        public int CountTotal { get; set; }
    }
    public class GetGameId




        
    {

        public string GetGameHash(string name)
        {
            return Crc64.Compute(name); // "5e05cccac2c36e68"
        }

        // Реализация CRC64 вручную, так как System.IO.Hashing.Crc64 отсутствует в большинстве версий .NET

    }

    public static class Crc64
    {
        private const ulong Poly = 0x95AC9329AC4BC9B5; // Специальный полином для HowLongToBeat
        private static readonly ulong[] Table = new ulong[256];

        static Crc64()
        {
            for (ulong i = 0; i < 256; i++)
            {
                ulong crc = i << 56;
                for (int j = 0; j < 8; j++)
                {
                    if ((crc & 0x8000000000000000) != 0)
                        crc = (crc << 1) ^ Poly;
                    else
                        crc <<= 1;
                }
                Table[i] = crc;
            }
        }

        public static string Compute(string input)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            ulong crc = 0;

            foreach (byte b in bytes)
            {
                byte index = (byte)((crc >> 56) ^ b);
                crc = Table[index] ^ (crc << 8);
            }

            // Конвертируем в big-endian и форматируем
            byte[] result = BitConverter.GetBytes(crc);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(result);
            }

            return BitConverter.ToString(result).Replace("-", "").ToLowerInvariant();
        }
    }
}
