using GameStore.Api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Data;

//Update migrations every time the program runs
public static class DataExtentsions
{
    public static async Task IntitalizeDbAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<GameStoreContext>();
        await dbContext.Database.MigrateAsync();
    }

    public static IServiceCollection AddRepositories(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connString = configuration.GetConnectionString("GameStoreContext");
        services.AddSqlServer<GameStoreContext>(connString)
                .AddScoped<IGamesRepository, EntityFrameworkGamesRepository>();
        //Change from in memory to entity framework
        //.AddSingleton<IGamesRepository, InMemGamesRepository>();

        return services;
    }

}