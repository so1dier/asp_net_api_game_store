using GameStore.Api.Dtos;
using GameStore.Api.Entities;
using GameStore.Api.Repositories;

namespace GameStore.Api.Endpoints;


public static class GamesEndpoints
{
    const string GetGameEndpointName = "GetGame";


    public static RouteGroupBuilder MapGamesEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/games")
                        .WithParameterValidation();

        //group.MapGet("/", () => "Hello World!");
        group.MapGet("/", (IGamesRepository repository) => 
            repository.GetAll().Select(game => game.AsDto()));

        group.MapGet("/{id}", (IGamesRepository repository, int id) =>
        {
            Game? game = repository.Get(id);
            return game is null ? Results.NotFound() : Results.Ok(game.AsDto());
        })
        .WithName(GetGameEndpointName);

        group.MapPost("/", (IGamesRepository repository, CreateGameDto gameDto) =>
        {
            Game game = new()
            {
                Name = gameDto.Name,
                Genre = gameDto.Genre,
                Price = gameDto.Price, 
                ReleaseDate = gameDto.ReleaseDate, 
                ImageUri = gameDto.ImageUri
            };

            repository.Create(game);

            return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);
        });

        group.MapPut("/{id}", (IGamesRepository repository, int id, UpdateGameDto updatedGameDto) =>
        {
            Game? exisingGame = repository.Get(id);

            if (exisingGame is null)
            {
                return Results.NotFound();
            }

            exisingGame.Name = updatedGameDto.Name;
            exisingGame.Genre = updatedGameDto.Genre;
            exisingGame.ImageUri = updatedGameDto.ImageUri;
            exisingGame.Price = updatedGameDto.Price;
            exisingGame.ReleaseDate = updatedGameDto.ReleaseDate;

            repository.Update(exisingGame);

            return Results.NoContent();
        });

        group.MapDelete("/{id}", (IGamesRepository repository, int id) =>
        {
            Game? game = repository.Get(id);

            if (game is not null)
            {
                repository.Delete(id);
            }

            return Results.NoContent();
        });

        return group;
    }
}