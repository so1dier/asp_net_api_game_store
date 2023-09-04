using GameStore.Api.Authorization;
using GameStore.Api.Dtos;
using GameStore.Api.Entities;
using GameStore.Api.Repositories;

namespace GameStore.Api.Endpoints;


public static class GamesEndpoints
{
    const string GetGameV1EndpointName = "GetGameV1";
    const string GetGameV2EndpointName = "GetGameV2";

    public static RouteGroupBuilder MapGamesEndpoints(this IEndpointRouteBuilder routes)
    {
        var v1Group = routes.MapGroup("/v1/games")
                        .WithParameterValidation();

        var v2Group = routes.MapGroup("/v2/games")
                        .WithParameterValidation();

        //group.MapGet("/", () => "Hello World!");
        //V1 GET endpoints
        v1Group.MapGet("/", async (IGamesRepository repository, ILoggerFactory loggerFactory) =>
        {
            return Results.Ok((await repository.GetAllAsync()).Select(game => game.AsDtoV1()));
        });

        v1Group.MapGet("/{id}", async (IGamesRepository repository, int id) =>
        {
            Game? game = await repository.GetAsync(id);
            return game is null ? Results.NotFound() : Results.Ok(game.AsDtoV1());
        })
        .WithName(GetGameV1EndpointName)
        .RequireAuthorization(Policies.ReadAccess);

        //V2 GET endpoints
        v2Group.MapGet("/", async (IGamesRepository repository, ILoggerFactory loggerFactory) =>
        {
            return Results.Ok((await repository.GetAllAsync()).Select(game => game.AsDtoV2()));
        });

        v2Group.MapGet("/{id}", async (IGamesRepository repository, int id) =>
        {
            Game? game = await repository.GetAsync(id);
            return game is null ? Results.NotFound() : Results.Ok(game.AsDtoV2());
        })
        .WithName(GetGameV2EndpointName)
        .RequireAuthorization(Policies.ReadAccess);

        v1Group.MapPost("/", async (IGamesRepository repository, CreateGameDto gameDto) =>
        {
            Game game = new()
            {
                Name = gameDto.Name,
                Genre = gameDto.Genre,
                Price = gameDto.Price,
                ReleaseDate = gameDto.ReleaseDate,
                ImageUri = gameDto.ImageUri
            };

            await repository.CreateAsync(game);

            return Results.CreatedAtRoute(GetGameV1EndpointName, new { id = game.Id }, game);
        })
        .RequireAuthorization(Policies.WriteAccess);

        v1Group.MapPut("/{id}", async (IGamesRepository repository, int id, UpdateGameDto updatedGameDto) =>
        {
            Game? exisingGame = await repository.GetAsync(id);

            if (exisingGame is null)
            {
                return Results.NotFound();
            }

            exisingGame.Name = updatedGameDto.Name;
            exisingGame.Genre = updatedGameDto.Genre;
            exisingGame.ImageUri = updatedGameDto.ImageUri;
            exisingGame.Price = updatedGameDto.Price;
            exisingGame.ReleaseDate = updatedGameDto.ReleaseDate;

            await repository.UpdateAsync(exisingGame);

            return Results.NoContent();
        })
        .RequireAuthorization(Policies.WriteAccess);

        v1Group.MapDelete("/{id}", async (IGamesRepository repository, int id) =>
        {
            Game? game = await repository.GetAsync(id);

            if (game is not null)
            {
                await repository.DeleteAsync(id);
            }

            return Results.NoContent();
        })
        .RequireAuthorization(Policies.WriteAccess);

        return v1Group;
    }
}