using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TiktokGame2Server.Others;

public class TokenAuthMiddleware
{
    private readonly RequestDelegate _next;

    public TokenAuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ITokenService tokenService)
    {
        // ֻ��֤API�ӿڣ��ɸ���ʵ���������·�����ˣ�
        if (context.Request.Path.StartsWithSegments("/api") || context.Request.Path.StartsWithSegments("/Game"))
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault();

            if (string.IsNullOrEmpty(token) || !tokenService.ValidateToken(token))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Token��Ч��δ�ṩ");
                return;
            }
        }

        await _next(context);
    }
}