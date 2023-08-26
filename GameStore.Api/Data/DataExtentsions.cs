using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Data;

//Update migrations every time the program runs
public static class DataExtentsions
{
    public static void IntitalizeDb(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<GameStoreContext>();
        dbContext.Database.Migrate();
    }
    
}