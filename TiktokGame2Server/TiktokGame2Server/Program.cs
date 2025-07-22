using JFramework;
using JFramework.Game;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
    options.JsonSerializerOptions.PropertyNamingPolicy = null; // ����ԭ��
    // �� options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase; // ����ĸСд
});

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(7289, listenOptions =>
    {
        listenOptions.UseHttps(); // Ĭ��ʹ�ÿ���֤��
    });
    options.ListenAnyIP(5289); // http
});

builder.Services.AddSwaggerWithJwt();


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

//builder.Services.AddIdentity<Account, IdentityRole>(options =>
//{
//    options.User.RequireUniqueEmail = false; // ��Ҫ��Ψһ���䣬Ҳ��Ҫ���ṩ����
//});


//builder.Services.adda



builder.Services.AddDbContext<MyDbContext>(options => options.UseNpgsql("Server=localhost;Port=5432;Database=MyDb;UserId=postgres;Password=123321qweasd;"));

// ����JWT��֤
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


var app = builder.Build();


var configService = app.Services.GetRequiredService<TiktokConfigService>();
await configService.PreloadAllAsync("D:/Demos/TiktokGame2/Assets/Downloads/GameRes/Dynamic/Configs/", ".json"); // ��� Main �� async Task

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

app.Run();
