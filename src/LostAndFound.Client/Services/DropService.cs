using System.Net.Http.Json;
using LostAndFound.Shared.DTOs;

namespace LostAndFound.Client.Services;

public class DropService
{
    private readonly HttpClient _httpClient;

    public DropService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<DropResponse?> CreateDropAsync(CreateDropRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/drops", request);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<DropResponse>();
        }
        return null;
    }

    public async Task<DropResponse?> FindRandomAsync(string? category = null)
    {
        var url = "api/drops/random";
        if (!string.IsNullOrEmpty(category))
        {
            url += $"?category={category}";
        }

        var response = await _httpClient.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<DropResponse>();
        }
        return null;
    }

    public async Task<StatsResponse?> GetStatsAsync()
    {
        return await _httpClient.GetFromJsonAsync<StatsResponse>("api/drops/stats");
    }
}

public class StatsResponse
{
    public int TotalDrops { get; set; }
    public int TotalFinds { get; set; }
    public List<CategoryStat> ByCategory { get; set; } = new();
}

public class CategoryStat
{
    public string Category { get; set; } = string.Empty;
    public int Count { get; set; }
}
