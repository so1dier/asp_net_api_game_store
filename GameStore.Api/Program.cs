using GameStore.Api.Entities;

const string GetGameEndpointName = "GetGame";

List<Game> games = new()
{
    new Game()
    {
        Id = 1,
        Name = "Street Fighter II",
        Genre = "Fighting",
        Price = 19.99M,
        ReleaseDate = new DateTime(1991, 2, 1),
        ImageUri = "https://placehold.co/100"
    },
    new Game()
    {
        Id = 2,
        Name = "Final Fantasy XIV",
        Genre = "Fantasy",
        Price = 59.99M,
        ReleaseDate = new DateTime(1991, 2, 1),
        ImageUri = "https://placehold.co/100"
    },
    new Game()
    {
        Id = 3,
        Name = "FIFA 23",
        Genre = "Sports",
        Price = 69.99M,
        ReleaseDate = new DateTime(2022, 9, 27),
        ImageUri = "https://placehold.co/100"
    }
};

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var group = app.MapGroup("/games");

app.MapGet("/", () => "Hello World!");
group.MapGet("/", () => games);
group.MapGet("/{id}", (int id) =>
{
    Game? game = games.Find(game => game.Id == id);

    if (game is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(game);
})
.WithName(GetGameEndpointName);

group.MapPost("/", (Game game) =>
{
    game.Id = games.Max(game => game.Id) + 1;
    games.Add(game);

    return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);
});

group.MapPut("/{id}", (int id, Game game) =>
{
    Game? exisingGame = games.Find(game => game.Id == id);

    if (exisingGame is null)
    {
        return Results.NotFound();
    }

    exisingGame.Name = game.Name;
    exisingGame.Genre = game.Genre;
    exisingGame.ImageUri = game.ImageUri;
    exisingGame.Price = game.Price;
    exisingGame.ReleaseDate = game.ReleaseDate;

    return Results.NoContent();
});

group.MapDelete("/{id}", (int id) =>
{
    Game? game = games.Find(game => game.Id == id);

    if (game is not null)
    {
        games.Remove(game);
    }

    return Results.NoContent();
});

app.Run();
