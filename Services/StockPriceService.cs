using System.Net.Http.Json;
using System.Text.Json;
using StockPriceApi.Models;

namespace StockPriceApi.Services;

public class StockPriceService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiUrl;

    public StockPriceService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _apiUrl = config["ExternalApis:StockPriceApiUrl"]
                  ?? throw new Exception("StockPriceApiUrl not configured");
    }

    public async Task<List<StockPriceDto>> GetPriceDataAsync(string symbol, DateTime from, DateTime to)
    {
        var payload = new
        {
            action = "get_history",
            ticker = symbol,
            start_date = from.ToString("yyyy-MM-dd"),
            end_date = to.ToString("yyyy-MM-dd")
        };

        var response = await _httpClient.PostAsJsonAsync(_apiUrl, payload);
        if (!response.IsSuccessStatusCode)
            return [];

        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);

        if (!doc.RootElement.TryGetProperty("data", out var dataElement))
            return [];

        var result = new List<StockPriceDto>();
        var rows = dataElement.EnumerateArray().ToList();

        if (rows.Count <= 1)
            return result;

        // Skip header: ["Date", "Close"]
        foreach (var row in rows.Skip(1))
        {
            if (row.GetArrayLength() != 2) continue;

            var dateStr = row[0].ToString();
            var closeStr = row[1].ToString();

            if (decimal.TryParse(closeStr, out var close))
            {
                result.Add(new StockPriceDto
                {
                    Date = dateStr,
                    Close = close
                });
            }
        }

        return result;
    }
}
