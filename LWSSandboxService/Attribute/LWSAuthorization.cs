using System.Diagnostics.CodeAnalysis;
using LWSSandboxService.Model;
using LWSSandboxService.Model.Response;
using LWSSandboxService.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LWSSandboxService.Attribute;

[ExcludeFromCodeCoverage]
public class LwsAuthorization : ActionFilterAttribute
{
    public AccountRole TargetAccountRole { get; set; }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var httpContext = context.HttpContext;
        var authService = httpContext.RequestServices.GetService<IAuthorizationService>();
        if (!httpContext.Request.Headers.TryGetValue("X-LWS-AUTH", out var token))
        {
            context.Result = new UnauthorizedObjectResult(new ErrorResponse
            {
                StatusCodes = StatusCodes.Status401Unauthorized,
                Message = "This API needs to be logged-in. Please login!",
                ErrorPath = context.HttpContext.Request.Path
            });
        }

        var account = authService.AuthorizeAsync(token)
            .GetAwaiter().GetResult();
        if (account?.Roles.Contains(TargetAccountRole) == true)
        {
            httpContext.Items.Add("accountId", account.UserId);
        }
        else
        {
            context.Result = new UnauthorizedObjectResult(new ErrorResponse
            {
                StatusCodes = StatusCodes.Status401Unauthorized,
                Message = "This API needs to be logged-in. Please login!",
                ErrorPath = context.HttpContext.Request.Path
            });
        }

        base.OnActionExecuting(context);
    }
}