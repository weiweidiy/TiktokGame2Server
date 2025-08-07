using JFramework;
using JFramework.Game;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TiktokGame2Server.Entities;
using TiktokGame2Server.Others;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});


builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null; // 保持原样
    // 或 options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase; // 首字母小写
});

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(7289, listenOptions =>
    {
        listenOptions.UseHttps(); // 默认使用开发证书
    });
    options.ListenAnyIP(5289); // http
});

builder.Services.AddSwaggerWithJwt();



builder.Services.AddDbContext<MyDbContext>(options => options.UseNpgsql("Server=localhost;Port=5432;Database=MyDb;UserId=postgres;Password=123321qweasd;"));


builder.Services.AddSingleton<IDeserializer, JsonNetDeserilizer>();
builder.Services.AddSingleton<IConfigLoader, LocalFileConfigLoader>();
builder.Services.AddSingleton<TiktokConfigService>();


builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IPlayerService, PlayerService>();
builder.Services.AddScoped<ILevelNodesService, LevelNodeService>();
builder.Services.AddScoped<ISamuraiService, SamuraiService>();
builder.Services.AddScoped<IFormationService, FormationService>();
builder.Services.AddScoped<ILevelNodeCombatService, LevelNodeCombatService>();
builder.Services.AddScoped<IHpPoolService, HpPoolService>();
builder.Services.AddScoped<IAchievementService, AchievementService>();
builder.Services.AddScoped<IEvaluationService, TiktokCombatEvaluationService>();
builder.Services.AddScoped<ICurrencyService, CurrencyService>();
builder.Services.AddScoped<IBagService, BagService>();
builder.Services.AddScoped<IRewardService, RewardService>();
builder.Services.AddScoped<IDrawSamuraiService, DrawSamuraiService>();


// 配置JWT认证
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = true,
//            ValidateAudience = true,
//            ValidateLifetime = true,
//            ValidateIssuerSigningKey = true,
//            ValidIssuer = builder.Configuration["Jwt:Issuer"],
//            ValidAudience = builder.Configuration["Jwt:Audience"],
//            IssuerSigningKey = new SymmetricSecurityKey(
//                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
//        };
//    });

// 添加日志（默认已包含Console、Debug等）
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
//builder.Logging.AddFile("Logs/app-{Date}.log"); // 需安装第三方包，如 Serilog 或 Microsoft.Extensions.Logging.File

var app = builder.Build();


var configService = app.Services.GetRequiredService<TiktokConfigService>();

//"D:\\Demos\\TiktokGame2Server\\TiktokGame2Server\\TiktokGame2Server\\Configs\\"
//"E:\\UnityProjects\\TiktokGame2Server\\TiktokGame2Server\\TiktokGame2Server\\Configs\\"

await configService.PreloadAllAsync("D:\\Demos\\TiktokGame2Server\\TiktokGame2Server\\TiktokGame2Server\\Configs\\", ".json"); // 如果 Main 是 async Task

app.UseMiddleware<TokenAuthMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//app.MapGet("/", ()=>"hello world");

//app.UseExceptionHandler(errorApp =>
//{
//    errorApp.Run(async context =>
//    {
//        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
//        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
//        if (exceptionHandlerPathFeature?.Error != null)
//        {
//            logger.LogError(exceptionHandlerPathFeature.Error, "Unhandled exception occurred.");
//        }
//        context.Response.StatusCode = 500;
//        await context.Response.WriteAsync("An error occurred.");
//    });
//});

app.Run();
