using Catalog.API;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddCatalogApi(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapCarter();

app.UseExceptionHandler(_ => { });

app.Run();
