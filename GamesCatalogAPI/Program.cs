using ApiRepos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repos;
using Services;
using Services.Functions;
using Services.IGDB;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

#region Conn config

string gamesCatalogConn = builder.Configuration.GetConnectionString("GamesCatalogConn");

builder.Services.AddDbContextFactory<DbCtx>(options
    => options.UseMySql(gamesCatalogConn, ServerVersion.AutoDetect(gamesCatalogConn)));

#endregion

#region Servs

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IEncryptionService, EncryptionService>(p
    => new EncryptionService(builder.Configuration["Encryption:Key32"], builder.Configuration["Encryption:IV16"]));

builder.Services.AddScoped<IJwtTokenService, JwtTokenService>(p
=> new JwtTokenService(builder.Configuration["JwtKey"]));

string emailUrl;

if (builder.Environment.IsDevelopment())
    emailUrl = builder.Configuration["SendEmailKeys:UrlLocal"];
else emailUrl = builder.Configuration["SendEmailKeys:UrlLocal"];

builder.Services.AddScoped<ISendRecoverPasswordEmailService, SendRecoverPasswordEmailService>(p
    => new SendRecoverPasswordEmailService(
    builder.Configuration["SendEmailKeys:SenderEmail"],
    emailUrl,
    builder.Configuration["SendEmailKeys:SenderPassword"],
    builder.Configuration["SendEmailKeys:Host"]
    ));

builder.Services.AddScoped<IIGDBGamesApiService>(provider =>
{
    var clientID = builder.Configuration["ClientID"];
    var accessTokenService = provider.GetRequiredService<IIGDBAccessTokenService>();
    return new IGDBGamesApiService(clientID, accessTokenService);
});

builder.Services.AddScoped<IIGDBAccessTokenService, IGDBAccessTokenService>();

builder.Services.AddScoped<IGameStatusService, GameStatusService>();

#endregion

#region Repos

builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<IIGDBAccessTokenRepo, IGDBAccessTokenRepo>();
builder.Services.AddScoped<IGameRepo, GameRepo>();
builder.Services.AddScoped<IGameStatusRepo, GameStatusRepo>();

#endregion

#region API Repos

builder.Services.AddScoped<IIGDBAccessTokenAPIRepo, IGDBAccessTokenAPIRepo>(p
    => new IGDBAccessTokenAPIRepo(
    builder.Configuration["ClientID"],
    builder.Configuration["ClientSecret"]
    ));

#endregion

#region Auth

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = false,
        ValidateIssuer = false,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtKey"]))
    };
    options.SaveToken = true;
});

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
    app.MapOpenApi();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
