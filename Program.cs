using StockPriceApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddScoped<StockPriceService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseStaticFiles(); // Serve static files from wwwroot

app.UseAuthorization();
app.MapControllers();
app.Run();
