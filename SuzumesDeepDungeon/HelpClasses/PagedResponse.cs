using System.Text.Json.Serialization;

namespace SuzumesDeepDungeon.HelpClasses;

public class PagedResponse<T>
{
    [JsonPropertyName("items")]
    public List<T> Items { get; set; }

    [JsonPropertyName("page")]
    public int Page { get; set; }

    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; }

    [JsonPropertyName("totalCount")]
    public int TotalCount { get; set; }

    [JsonPropertyName("totalPages")]
    public int TotalPages { get; set; }
}