using AngleSharp;
using AngleSharp.Dom;
using Fastenshtein;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

public class HowLongToBeatService
{
    private readonly HttpClient _httpClient;
    private readonly HltbSearch _hltb = new HltbSearch();

    public HowLongToBeatService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<HowLongToBeatEntry> Detail(string gameId, CancellationToken cancellationToken = default)
    {
        string detailPage = await _hltb.DetailHtml(gameId, _httpClient, cancellationToken);
        return HowLongToBeatParser.ParseDetails(detailPage, gameId);
    }

    public async Task<List<HowLongToBeatEntry>> Search(string query, CancellationToken cancellationToken = default)
    {
        string[] searchTerms = query.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        HltbSearchResult search = await _hltb.Search(searchTerms, _httpClient, cancellationToken);

        return search.Data.Select(resultEntry =>
        {
            var timeLabels = new List<List<string>>
            {
                new List<string> { "Main", "Main" },
                new List<string> { "Main + Extra", "Main + Extra" },
                new List<string> { "Completionist", "Completionist" }
            };

            return new HowLongToBeatEntry(
                id: resultEntry.GameId.ToString(),
                name: resultEntry.GameName,
                description: string.Empty,
                platforms: resultEntry.ProfilePlatform?.Split(", ").ToList() ?? new List<string>(),
                imageUrl: HltbSearch.IMAGE_URL + resultEntry.GameImage,
                timeLabels: timeLabels,
                gameplayMain: Math.Round(resultEntry.CompMain / 3600.0, 1),
                gameplayMainExtra: Math.Round(resultEntry.CompPlus / 3600.0, 1),
                gameplayCompletionist: Math.Round(resultEntry.Comp100 / 3600.0, 1),
                similarity: CalcDistancePercentage(resultEntry.GameName, query),
                searchTerm: query
            );
        }).ToList();
    }

    private static double CalcDistancePercentage(string text, string term)
    {
        string longer = text.Trim().ToLowerInvariant();
        string shorter = term.Trim().ToLowerInvariant();

        if (longer.Length < shorter.Length)
            (longer, shorter) = (shorter, longer);

        if (longer.Length == 0)
            return 1.0;

        int distance = Levenshtein.Distance(longer, shorter);
        return Math.Round((longer.Length - (double)distance) / longer.Length, 2);
    }
}

public class HowLongToBeatEntry
{
    [Obsolete("Use Platforms instead")]
    public List<string> PlayableOn => Platforms;

    public HowLongToBeatEntry(
        string id,
        string name,
        string description,
        List<string> platforms,
        string imageUrl,
        List<List<string>> timeLabels,
        double gameplayMain,
        double gameplayMainExtra,
        double gameplayCompletionist,
        double similarity,
        string searchTerm)
    {
        Id = id;
        Name = name;
        Description = description;
        Platforms = platforms;
        ImageUrl = imageUrl;
        TimeLabels = timeLabels;
        GameplayMain = gameplayMain;
        GameplayMainExtra = gameplayMainExtra;
        GameplayCompletionist = gameplayCompletionist;
        Similarity = similarity;
        SearchTerm = searchTerm;
    }

    public string Id { get; }
    public string Name { get; }
    public string Description { get; }
    public List<string> Platforms { get; }
    public string ImageUrl { get; }
    public List<List<string>> TimeLabels { get; }
    public double GameplayMain { get; }
    public double GameplayMainExtra { get; }
    public double GameplayCompletionist { get; }
    public double Similarity { get; }
    public string SearchTerm { get; }
}

public static class HowLongToBeatParser
{
    public static HowLongToBeatEntry ParseDetails(string html, string id)
    {
        var context = BrowsingContext.New(Configuration.Default);
        var document = context.OpenAsync(req => req.Content(html)).Result;

        string gameName = document.QuerySelector("div[class*=GameHeader_profile_header__]")
            ?.FirstChild?.TextContent?.Trim() ?? string.Empty;

        string imageUrl = document.QuerySelector("div[class*=GameHeader_game_image__] img")
            ?.GetAttribute("src") ?? string.Empty;

        string description = document.QuerySelector(".in.back_primary.shadow_box div[class*=GameSummary_large__]")
            ?.TextContent?.Trim() ?? string.Empty;

        var platforms = new List<string>();
        var platformElements = document.QuerySelectorAll("div[class*=GameSummary_profile_info__]");
        foreach (var element in platformElements)
        {
            if (element.TextContent.Contains("Platforms:"))
            {
                platforms = element.TextContent
                    .Replace("\n", "")
                    .Replace("Platforms:", "", StringComparison.OrdinalIgnoreCase)
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(p => p.Trim())
                    .ToList();
                break;
            }
        }

        double gameplayMain = 0;
        double gameplayMainExtra = 0;
        double gameplayComplete = 0;
        var timeLabels = new List<List<string>>();

        var liElements = document.QuerySelectorAll("div[class*=GameStats_game_times__] li");
        foreach (var li in liElements)
        {
            string type = li.QuerySelector("h4")?.TextContent?.Trim() ?? "";
            string timeText = li.QuerySelector("h5")?.TextContent?.Trim() ?? "";
            double time = ParseTime(timeText);

            if (type.StartsWith("Main Story") ||
                type.StartsWith("Single-Player") ||
                type.StartsWith("Solo"))
            {
                gameplayMain = time;
                timeLabels.Add(new List<string> { "gameplayMain", type });
            }
            else if (type.StartsWith("Main + Sides") || type.StartsWith("Co-Op"))
            {
                gameplayMainExtra = time;
                timeLabels.Add(new List<string> { "gameplayMainExtra", type });
            }
            else if (type.StartsWith("Completionist") || type.StartsWith("Vs."))
            {
                gameplayComplete = time;
                timeLabels.Add(new List<string> { "gameplayCompletionist", type });
            }
        }

        return new HowLongToBeatEntry(
            id,
            gameName,
            description,
            platforms,
            imageUrl,
            timeLabels,
            gameplayMain,
            gameplayMainExtra,
            gameplayComplete,
            1.0,
            gameName
        );
    }

    private static double ParseTime(string text)
    {
        if (text.StartsWith("--")) return 0;
        return text.Contains(" - ") ? HandleRange(text) : GetTime(text);
    }

    private static double HandleRange(string text)
    {
        var parts = text.Split(new[] { " - " }, StringSplitOptions.RemoveEmptyEntries);
        return (GetTime(parts[0]) + GetTime(parts[1])) / 2;
    }

    private static double GetTime(string text)
    {
        if (text.EndsWith("Mins", StringComparison.OrdinalIgnoreCase))
            return 0.5;

        string timePart = text.Split(' ')[0];

        if (timePart.Contains('½'))
            return double.Parse(timePart[..timePart.IndexOf('½')]) + 0.5;

        return double.TryParse(timePart, out double result) ? result : 0;
    }
}

public class HltbSearch
{
    public const string BASE_URL = "https://howlongtobeat.com/";
    public const string DETAIL_URL = $"{BASE_URL}game?id=";
    public const string SEARCH_URL = $"{BASE_URL}api/seek";
    public const string IMAGE_URL = $"{BASE_URL}games/";

    private static readonly string[] UserAgents = {
        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/125.0.0.0 Safari/537.36",
        "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/17.0 Safari/605.1.15",
        "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:126.0) Gecko/20100101 Firefox/126.0"
    };

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly Random _random = new();

    private readonly Dictionary<string, object> _payload = new()
    {
        ["searchType"] = "games",
        ["searchTerms"] = new List<string>(),
        ["searchPage"] = 1,
        ["size"] = 20,
        ["searchOptions"] = new Dictionary<string, object>
        {
            ["games"] = new Dictionary<string, object>
            {
                ["userId"] = 0,
                ["platform"] = "",
                ["sortCategory"] = "popular",
                ["rangeCategory"] = "main",
                ["rangeTime"] = new Dictionary<string, int> { ["min"] = 0, ["max"] = 0 },
                ["gameplay"] = new Dictionary<string, string>
                {
                    ["perspective"] = "",
                    ["flow"] = "",
                    ["genre"] = ""
                },
                ["modifier"] = ""
            },
            ["users"] = new Dictionary<string, string>
            {
                ["sortCategory"] = "postcount"
            },
            ["filter"] = "",
            ["sort"] = 0,
            ["randomizer"] = 0
        }
    };

    public async Task<string> DetailHtml(string gameId, HttpClient httpClient, CancellationToken cancellationToken = default)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{DETAIL_URL}{gameId}");
            AddRequestHeaders(request);

            using var response = await httpClient.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync(cancellationToken);
        }
        catch (TaskCanceledException)
        {
            throw new OperationCanceledException("Request was canceled");
        }
        catch (HttpRequestException ex)
        {
            throw new Exception($"HTTP error: {ex.StatusCode}", ex);
        }
    }

    public async Task<HltbSearchResult> Search(string[] searchTerms, HttpClient httpClient, CancellationToken cancellationToken = default)
    {
        try
        {
            var payload = ClonePayload();
            payload["searchTerms"] = searchTerms;

            var json = JsonSerializer.Serialize(payload, JsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, SEARCH_URL)
            {
                Content = content
            };
            AddRequestHeaders(request);

            using var response = await httpClient.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync(cancellationToken);
            return JsonSerializer.Deserialize<HltbSearchResult>(responseJson, JsonOptions);
        }
        catch (TaskCanceledException)
        {
            throw new OperationCanceledException("Search was canceled");
        }
        catch (HttpRequestException ex)
        {
            throw new Exception($"Search failed: {ex.StatusCode}", ex);
        }
    }

    private Dictionary<string, object> ClonePayload()
    {
        var serialized = JsonSerializer.Serialize(_payload);
        return JsonSerializer.Deserialize<Dictionary<string, object>>(serialized);
    }

    private void AddRequestHeaders(HttpRequestMessage request)
    {
        var randomAgent = UserAgents[_random.Next(UserAgents.Length)];

        request.Headers.Add("User-Agent", randomAgent);
        request.Headers.Add("Origin", BASE_URL);
        request.Headers.Add("Referer", BASE_URL);
        request.Headers.Add("Accept", "application/json");
    }
}


public class HltbSearchResult
{
    public int Count { get; set; }
    public List<HltbGame> Data { get; set; } = new List<HltbGame>();
}

public class HltbGame
{
    public int GameId { get; set; }
    public string GameName { get; set; }
    public string ProfilePlatform { get; set; }
    public string GameImage { get; set; }
    public int CompMain { get; set; }
    public int CompPlus { get; set; }
    public int Comp100 { get; set; }
}