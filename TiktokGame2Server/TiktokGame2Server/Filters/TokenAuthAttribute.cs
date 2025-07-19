using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TiktokGame2Server.Others;

public class TokenAuthAttribute : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var tokenService = context.HttpContext.RequestServices.GetService(typeof(ITokenService)) as ITokenService;

        // ֻ������ͷ��ȡ token
        var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault();

        if (string.IsNullOrEmpty(token) || tokenService == null || !tokenService.ValidateToken(token))
        {
            context.Result = new UnauthorizedObjectResult("Token��Ч��δ�ṩ");
            return;
        }

        await next();
    }
}