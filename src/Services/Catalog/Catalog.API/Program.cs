
using Catalog.API;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddCatalogApi(builder.Configuration);

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapCarter();

app.UseExceptionHandler(options => { });

app.Run();
