using Basket.API;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services
    .AddBasketAPI(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapCarter();
app.UseExceptionHandler(_ => { });
app.Run();
