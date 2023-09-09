namespace GameStore.Api.Cors;

public static class CorsExtensions
{
    private const string allowedOriginSetting = "AllowedOrigin";

    public static IServiceCollection AddGameStoreCors(
        this IServiceCollection service, IConfiguration configuration)
    {
        return service.AddCors(options =>
        {
            options.AddDefaultPolicy(corsBuilder =>
            {
                var allowedOrigin = configuration[allowedOriginSetting]
                    ?? throw new InvalidOperationException("Allowed origin is not set");
                corsBuilder.WithOrigins(allowedOrigin)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .WithExposedHeaders("X-Pagination");
            });
        });
    }
}