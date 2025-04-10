using Microsoft.EntityFrameworkCore;
using Repos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

#region Conn config

string gamesCatalogConn = builder.Configuration.GetConnectionString("GamesCatalogConn");

builder.Services.AddMySql<DbCtx>(gamesCatalogConn, ServerVersion.AutoDetect(gamesCatalogConn));

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
