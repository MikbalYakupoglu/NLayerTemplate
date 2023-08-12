using Core.Entity.Temp;
using Microsoft.AspNetCore.Http;

namespace Core.Extensions;

public class GetLoginedUserMiddleware
{
    private RequestDelegate _next;

    public GetLoginedUserMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        if (httpContext.User.Identities.FirstOrDefault().Claims.Count() == 0)
        {
            await _next(httpContext);
        }
        else
        {
            LoginedUser.ClaimsPrincipal = httpContext.User;
            await _next(httpContext);
        }
    }
}