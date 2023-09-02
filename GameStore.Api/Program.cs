using GameStore.Api.Authorization;
using GameStore.Api.Data;
using GameStore.Api.Endpoints;
using GameStore.Api.Middlewawre;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRepositories(builder.Configuration);
builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddGameStoreAutorization();

builder.Logging.AddJsonConsole(options =>
{
    options.JsonWriterOptions = new()
    {
        Indented = true
    };
});

var app = builder.Build();

app.UseMiddleware<RequestTimingMiddleware>();

await app.Services.IntitalizeDbAsync();

app.UseHttpLogging();

app.MapGamesEndpoints();

app.Run();