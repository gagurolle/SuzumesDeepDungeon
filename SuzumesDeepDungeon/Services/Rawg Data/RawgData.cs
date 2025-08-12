namespace SuzumesDeepDungeon.Services
{
    using System;
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

        [JsonPropertyName("playtime")]
        public int Playtime { get; set; }

        [JsonPropertyName("platforms")]
        public List<PlatformWrapper> Platforms { get; set; }

        [JsonPropertyName("stores")]
        public List<StoreWrapper> Stores { get; set; }

        [JsonPropertyName("released")]
        public string Released { get; set; }

        [JsonPropertyName("tba")]
        public bool Tba { get; set; }

        [JsonPropertyName("background_image")]
        public string BackgroundImage { get; set; }

        [JsonPropertyName("rating")]
        public double Rating { get; set; }

        [JsonPropertyName("rating_top")]
        public int RatingTop { get; set; }

        [JsonPropertyName("ratings")]
        public List<Rating> Ratings { get; set; }

        [JsonPropertyName("ratings_count")]
        public int RatingsCount { get; set; }

        [JsonPropertyName("reviews_text_count")]
        public int ReviewsTextCount { get; set; }

        [JsonPropertyName("added")]
        public int Added { get; set; }

        [JsonPropertyName("added_by_status")]
        public AddedByStatus AddedByStatus { get; set; }

        [JsonPropertyName("metacritic")]
        public int? Metacritic { get; set; }

        [JsonPropertyName("suggestions_count")]
        public int SuggestionsCount { get; set; }

        [JsonPropertyName("updated")]
        public string Updated { get; set; }

        [JsonPropertyName("score")]
        public string Score { get; set; }

        [JsonPropertyName("clip")]
        public object Clip { get; set; }

        [JsonPropertyName("tags")]
        public List<Tag> Tags { get; set; }

        [JsonPropertyName("esrb_rating")]
        public EsrbRating EsrbRating { get; set; }

        [JsonPropertyName("user_game")]
        public object UserGame { get; set; }

        [JsonPropertyName("reviews_count")]
        public int ReviewsCount { get; set; }

        [JsonPropertyName("saturated_color")]
        public string SaturatedColor { get; set; }

        [JsonPropertyName("dominant_color")]
        public string DominantColor { get; set; }

        [JsonPropertyName("short_screenshots")]
        public List<ShortScreenshot> ShortScreenshots { get; set; }

        [JsonPropertyName("parent_platforms")]
        public List<ParentPlatform> ParentPlatforms { get; set; }

        [JsonPropertyName("genres")]
        public List<Genre> Genres { get; set; }
    }

    public class PlatformWrapper
    {
        [JsonPropertyName("platform")]
        public Platform Platform { get; set; }
    }

    public class Platform
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("slug")]
        public string Slug { get; set; }
    }

    public class StoreWrapper
    {
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
        public int ToPlay { get; set; }

        [JsonPropertyName("dropped")]
        public int Dropped { get; set; }

        [JsonPropertyName("playing")]
        public int Playing { get; set; }
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

    public class EsrbRating
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("slug")]
        public string Slug { get; set; }

        [JsonPropertyName("name_en")]
        public string NameEn { get; set; }

        [JsonPropertyName("name_ru")]
        public string NameRu { get; set; }
    }

    public class ShortScreenshot
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("image")]
        public string Image { get; set; }
    }

    public class ParentPlatform
    {
        [JsonPropertyName("platform")]
        public PlatformInfo Platform { get; set; }
    }

    public class PlatformInfo
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("slug")]
        public string Slug { get; set; }
    }

    public class Genre
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("slug")]
        public string Slug { get; set; }
    }
}
