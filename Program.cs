using StockPriceApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddScoped<StockPriceService>();

var app = builder.Build();

app.UseAuthorization();
app.MapControllers();
app.Run();
