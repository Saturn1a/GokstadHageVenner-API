namespace GokstadHageVennerAPI.Middleware;

public class GlobalExceptionMiddleware : IMiddleware
{
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(ILogger<GlobalExceptionMiddleware> logger)
    {
        _logger = logger;

    }


    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {

            _logger.LogError(ex, "Something went wrong {@MAchine} {@TraceId}",
                Environment.MachineName,
                System.Diagnostics.Activity.Current?.Id);

            await Results.Problem(
                title: "Something went wrong",
                statusCode: StatusCodes.Status500InternalServerError,
                extensions: new Dictionary<string, Object?>
                {
                    { "traceID", System.Diagnostics.Activity.Current?.Id },

                }).ExecuteAsync(context);
        }

    }
}
