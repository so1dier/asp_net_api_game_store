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
        group.MapGet("/", (IGamesRepository repository) => repository.GetAll());

        group.MapGet("/{id}", (IGamesRepository repository, int id) =>
        {
            Game? game = repository.Get(id);
            return game is null ? Results.NotFound() : Results.Ok(game);
        })
        .WithName(GetGameEndpointName);

        group.MapPost("/", (IGamesRepository repository, Game game) =>
        {
            repository.Create(game);

            return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);
        });

        group.MapPut("/{id}", (IGamesRepository repository, int id, Game game) =>
        {
            Game? exisingGame = repository.Get(id);

            if (exisingGame is null)
            {
                return Results.NotFound();
            }

            exisingGame.Name = game.Name;
            exisingGame.Genre = game.Genre;
            exisingGame.ImageUri = game.ImageUri;
            exisingGame.Price = game.Price;
            exisingGame.ReleaseDate = game.ReleaseDate;

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