using FamilyGames.Application.Interfaces;
using FamilyGames.Application.Services;
using FamilyGames.Domain.Interfaces;
using FamilyGames.Infrastructure.Data;
using FamilyGames.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ── Databas ──────────────────────────────────────────────────
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// ── Repositories – registreras via interface (DI-krav) ────────
builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
builder.Services.AddScoped<IMatchRepository,  MatchRepository>();

// ── Services – registreras via interface (DI-krav) ────────────
builder.Services.AddScoped<IPlayerService, PlayerService>();
builder.Services.AddScoped<IMatchService,  MatchService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ── CORS – tillåter Blazor WASM att anropa API:et ─────────────
builder.Services.AddCors(options =>
    options.AddPolicy("BlazorClient", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod()));

var app = builder.Build();

// ── Kör migrations automatiskt vid start ──────────────────────
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("BlazorClient");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

// Behövs för att integrationstester ska fungera
public partial class Program { }