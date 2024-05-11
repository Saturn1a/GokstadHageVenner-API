
using GokstadHageVennerAPI.Services.Interface;
using System.Diagnostics;

namespace GokstadHageVennerAPI.Middleware;

public class GokstadHageVennerBasicAuthentication : IMiddleware
{
    private readonly IMemberService _memberService;
    private readonly ILogger<GokstadHageVennerBasicAuthentication> _logger;

    public GokstadHageVennerBasicAuthentication(IMemberService memberService, ILogger<GokstadHageVennerBasicAuthentication> logger)
    {
        _memberService = memberService;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context.Request.Path.StartsWithSegments("/api/v1/Member/register") && context.Request.Method == "POST")
        {
            await next(context);
            return;
        }

        try
        {
            if (!context.Request.Headers.ContainsKey("Authorization"))
                throw new UnauthorizedAccessException("Authorization missing in HTTP header");


            var authHeader = context.Request.Headers.Authorization;

            string base64string = authHeader.ToString().Split(" ")[1];

            string member_password = DecodeBase64String(base64string);


            var arr = member_password.Split(":");
            string userName = arr[0];
            string password = arr[1];

            int MemberId = await _memberService.GetAuthenticatedIdAsync(userName, password);
            if (MemberId == 0)
            {
                throw new UnauthorizedAccessException("Access denied");
            }

            context.Items["MemberId"] = MemberId;

            await next(context);


        }

        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "Something went wrong {@MAchine} {@TraceId}",
                Environment.MachineName,
                System.Diagnostics.Activity.Current?.Id);

            // returned to user
            await Results.Problem(
                title: " Unauthorized: cant use this API",
                statusCode: StatusCodes.Status401Unauthorized,
                detail: ex.Message,
                extensions: new Dictionary<string, Object?>
                {
                    {"traceId", Activity.Current?.Id }

                })
                .ExecuteAsync(context);

        }

    }

    private string DecodeBase64String(string base64string)
    {
        byte[] base64bytes = System.Convert.FromBase64String(base64string);
        string username_and_password = System.Text.Encoding.UTF8.GetString(base64bytes);
        return username_and_password;

    }



}
