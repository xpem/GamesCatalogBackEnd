using Microsoft.EntityFrameworkCore;
using Repos;
using Services;
using Services.Functions;

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

#endregion

#region Repos

builder.Services.AddScoped<IUserRepo, UserRepo>();

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
