using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repos;
using Services;
using Services.Functions;
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

#endregion

#region Repos

builder.Services.AddScoped<IUserRepo, UserRepo>();

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
