using System.Collections.Generic;
using System.Text.Json.Serialization;

public class RawgApiResponse
{
    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("next")]
    public string Next { get; set; }

    [JsonPropertyName("previous")]
    public string Previous { get; set; }

    [JsonPropertyName("results")]
    public List<Game> Results { get; set; }

    [JsonPropertyName("user_platforms")]
    public bool UserPlatforms { get; set; }
}

public class Game
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("slug")]
    public string Slug { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("name_original")]
    public string NameOriginal { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("metacritic")]
    public int? Metacritic { get; set; }

    [JsonPropertyName("metacritic_platforms")]
    public List<MetacriticPlatform> MetacriticPlatforms { get; set; }

    [JsonPropertyName("released")]
    public string Released { get; set; }

    [JsonPropertyName("tba")]
    public bool Tba { get; set; }

    [JsonPropertyName("updated")]
    public string Updated { get; set; }

    [JsonPropertyName("background_image")]
    public string BackgroundImage { get; set; }

    [JsonPropertyName("background_image_additional")]
    public string BackgroundImageAdditional { get; set; }

    [JsonPropertyName("website")]
    public string Website { get; set; }

    [JsonPropertyName("rating")]
    public double Rating { get; set; }

    [JsonPropertyName("rating_top")]
    public int RatingTop { get; set; }

    [JsonPropertyName("ratings")]
    public List<Rating> Ratings { get; set; }

    [JsonPropertyName("reactions")]
    public Dictionary<string, int> Reactions { get; set; }

    [JsonPropertyName("added")]
    public int Added { get; set; }

    [JsonPropertyName("added_by_status")]
    public AddedByStatus AddedByStatus { get; set; }

    [JsonPropertyName("playtime")]
    public int Playtime { get; set; }

    [JsonPropertyName("screenshots_count")]
    public int ScreenshotsCount { get; set; }

    [JsonPropertyName("movies_count")]
    public int MoviesCount { get; set; }

    [JsonPropertyName("creators_count")]
    public int CreatorsCount { get; set; }

    [JsonPropertyName("achievements_count")]
    public int AchievementsCount { get; set; }

    [JsonPropertyName("parent_achievements_count")]
    public int ParentAchievementsCount { get; set; }

    [JsonPropertyName("reddit_url")]
    public string RedditUrl { get; set; }

    [JsonPropertyName("reddit_name")]
    public string RedditName { get; set; }

    [JsonPropertyName("reddit_description")]
    public string RedditDescription { get; set; }

    [JsonPropertyName("reddit_logo")]
    public string RedditLogo { get; set; }

    [JsonPropertyName("reddit_count")]
    public int RedditCount { get; set; }

    [JsonPropertyName("twitch_count")]
    public int TwitchCount { get; set; }

    [JsonPropertyName("youtube_count")]
    public int YoutubeCount { get; set; }

    [JsonPropertyName("reviews_text_count")]
    public int ReviewsTextCount { get; set; }

    [JsonPropertyName("ratings_count")]
    public int RatingsCount { get; set; }

    [JsonPropertyName("suggestions_count")]
    public int SuggestionsCount { get; set; }

    [JsonPropertyName("alternative_names")]
    public List<string> AlternativeNames { get; set; }

    [JsonPropertyName("metacritic_url")]
    public string MetacriticUrl { get; set; }

    [JsonPropertyName("parents_count")]
    public int ParentsCount { get; set; }

    [JsonPropertyName("additions_count")]
    public int AdditionsCount { get; set; }

    [JsonPropertyName("game_series_count")]
    public int GameSeriesCount { get; set; }

    [JsonPropertyName("user_game")]
    public object UserGame { get; set; }

    [JsonPropertyName("reviews_count")]
    public int ReviewsCount { get; set; }

    [JsonPropertyName("saturated_color")]
    public string SaturatedColor { get; set; }

    [JsonPropertyName("dominant_color")]
    public string DominantColor { get; set; }

    [JsonPropertyName("parent_platforms")]
    public List<ParentPlatform> ParentPlatforms { get; set; }

    [JsonPropertyName("platforms")]
    public List<PlatformInfo> Platforms { get; set; }

    [JsonPropertyName("stores")]
    public List<StoreInfo> Stores { get; set; }

    [JsonPropertyName("developers")]
    public List<Developer> Developers { get; set; }

    [JsonPropertyName("genres")]
    public List<Genre> Genres { get; set; }

    [JsonPropertyName("tags")]
    public List<Tag> Tags { get; set; }

    [JsonPropertyName("publishers")]
    public List<Publisher> Publishers { get; set; }

    [JsonPropertyName("esrb_rating")]
    public EsrbRating EsrbRating { get; set; }

    [JsonPropertyName("clip")]
    public object Clip { get; set; }

    [JsonPropertyName("description_raw")]
    public string DescriptionRaw { get; set; }
}

// Остальные классы с атрибутами JsonPropertyName
public class MetacriticPlatform
{
    [JsonPropertyName("metascore")]
    public int Metascore { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }

    [JsonPropertyName("platform")]
    public Platform Platform { get; set; }
}

public class Platform
{
    [JsonPropertyName("platform")]
    public int PlatformId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("slug")]
    public string Slug { get; set; }
}

public class Rating
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("percent")]
    public double Percent { get; set; }
}

public class AddedByStatus
{
    [JsonPropertyName("yet")]
    public int Yet { get; set; }

    [JsonPropertyName("owned")]
    public int Owned { get; set; }

    [JsonPropertyName("beaten")]
    public int Beaten { get; set; }

    [JsonPropertyName("toplay")]
    public int Toplay { get; set; }

    [JsonPropertyName("dropped")]
    public int Dropped { get; set; }

    [JsonPropertyName("playing")]
    public int Playing { get; set; }
}

public class ParentPlatform
{
    [JsonPropertyName("platform")]
    public Platform Platform { get; set; }
}

public class PlatformInfo
{
    [JsonPropertyName("platform")]
    public PlatformDetails Platform { get; set; }

    [JsonPropertyName("released_at")]
    public string ReleasedAt { get; set; }

    [JsonPropertyName("requirements")]
    public Requirements Requirements { get; set; }
}

public class PlatformDetails
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("slug")]
    public string Slug { get; set; }

    [JsonPropertyName("image")]
    public object Image { get; set; }

    [JsonPropertyName("year_end")]
    public object YearEnd { get; set; }

    [JsonPropertyName("year_start")]
    public object YearStart { get; set; }

    [JsonPropertyName("games_count")]
    public int GamesCount { get; set; }

    [JsonPropertyName("image_background")]
    public string ImageBackground { get; set; }
}

public class Requirements
{
    [JsonPropertyName("minimum")]
    public string Minimum { get; set; }

    [JsonPropertyName("recommended")]
    public string Recommended { get; set; }
}

public class StoreInfo
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }

    [JsonPropertyName("store")]
    public Store Store { get; set; }
}

public class Store
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("slug")]
    public string Slug { get; set; }

    [JsonPropertyName("domain")]
    public string Domain { get; set; }

    [JsonPropertyName("games_count")]
    public int GamesCount { get; set; }

    [JsonPropertyName("image_background")]
    public string ImageBackground { get; set; }
}

public class Developer
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("slug")]
    public string Slug { get; set; }

    [JsonPropertyName("games_count")]
    public int GamesCount { get; set; }

    [JsonPropertyName("image_background")]
    public string ImageBackground { get; set; }
}

public class Genre
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("slug")]
    public string Slug { get; set; }

    [JsonPropertyName("games_count")]
    public int GamesCount { get; set; }

    [JsonPropertyName("image_background")]
    public string ImageBackground { get; set; }
}

public class Tag
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("slug")]
    public string Slug { get; set; }

    [JsonPropertyName("language")]
    public string Language { get; set; }

    [JsonPropertyName("games_count")]
    public int GamesCount { get; set; }

    [JsonPropertyName("image_background")]
    public string ImageBackground { get; set; }
}

public class Publisher
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("slug")]
    public string Slug { get; set; }

    [JsonPropertyName("games_count")]
    public int GamesCount { get; set; }

    [JsonPropertyName("image_background")]
    public string ImageBackground { get; set; }
}

public class EsrbRating
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("slug")]
    public string Slug { get; set; }
}