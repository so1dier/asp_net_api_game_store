using System.Diagnostics;

namespace GameStore.Api.Middlewawre;

public class RequestTimingMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILogger<RequestTimingMiddleware> logger;

    public RequestTimingMiddleware(
        RequestDelegate next, 
        ILogger<RequestTimingMiddleware> logger)
    {
        this.next = next;
        this.logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
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
            logger.LogInformation(
                "{RequestMethod} {RequestPath} request took {ElapsedMiliseconds}ms to complete",
                context.Request.Method,
                context.Request.Path,
                elapsedMilliseconds);
        }
    }
}