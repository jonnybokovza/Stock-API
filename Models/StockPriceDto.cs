namespace StockPriceApi.Models
{
    public class StockPriceDto
    {
        public string Date { get; set; } = string.Empty;
        public decimal Close { get; set; }
    }
}
