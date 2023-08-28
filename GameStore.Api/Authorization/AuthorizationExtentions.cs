namespace GameStore.Api.Authorization;

public static class AuthorizationExtentions
{
    public static IServiceCollection AddGameStoreAutorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(Policies.ReadAccess,
                builder => builder.RequireClaim("scope", "games:read"));
            options.AddPolicy(Policies.WriteAccess,
                builder => builder.RequireClaim("scope", "games:write")
                                  .RequireRole("Admin"));
        });

        return services;
    }
}