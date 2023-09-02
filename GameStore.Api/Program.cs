using System.Diagnostics;
using GameStore.Api.Authorization;
using GameStore.Api.Data;
using GameStore.Api.Endpoints;
using GameStore.Api.Repositories;
using Microsoft.EntityFrameworkCore;

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


app.Use(async(context, next) =>
{
    var stopWatch = new Stopwatch();

    try
    {
        stopWatch.Start();
        await next(context);
    }
    finally
    {
        var elapsedMilliseconds = stopWatch.ElapsedMilliseconds;
        app.Logger.LogInformation(
            "{RequestMethod} {RequestPath} request took {ElapsedMiliseconds}ms to complete",
            context.Request.Method,
            context.Request.Path,
            elapsedMilliseconds);
    }

});

await app.Services.IntitalizeDbAsync();

app.UseHttpLogging();

app.MapGamesEndpoints();

app.Run();