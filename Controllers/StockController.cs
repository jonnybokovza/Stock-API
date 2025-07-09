using Microsoft.AspNetCore.Mvc;
using StockPriceApi.Services;

namespace StockPriceApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StockController : ControllerBase
{
    private readonly StockPriceService _stockPriceService;

    public StockController(StockPriceService stockPriceService)
    {
        _stockPriceService = stockPriceService;
    }

[HttpGet("{symbol}")]
public async Task<IActionResult> Get(string symbol, [FromQuery] DateTime from, [FromQuery] DateTime to)
{
    var data = await _stockPriceService.GetPriceDataAsync(symbol, from, to);

    if (data == null || data.Count == 0)
        return NotFound(new { message = "No data found or error occurred." });

    return Ok(new
    {
        symbol = symbol,
        from = from.ToString("yyyy-MM-dd"),
        to = to.ToString("yyyy-MM-dd"),
        price_history = data
    });
}

}
