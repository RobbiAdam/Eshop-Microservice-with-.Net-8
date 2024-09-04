using Basket.API;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services
    .AddApiServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseApiServices();

app.Run();
