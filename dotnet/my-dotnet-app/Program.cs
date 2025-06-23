using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

var builder = WebApplication.CreateBuilder(args);

// Leer variable de entorno para saber si estamos en DevSpaces
var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

string connectionString;

if (environment == "DevSpaces")
{
    // En DevSpaces, la base de datos ya está corriendo (definida en el devfile)
    // Usamos las variables de entorno para armar la cadena
    var host = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
    var port = Environment.GetEnvironmentVariable("DB_PORT") ?? "5432";
    var db = Environment.GetEnvironmentVariable("DB_NAME") ?? "moviesdb";
    var user = Environment.GetEnvironmentVariable("DB_USER") ?? "devuser";
    var password = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "devpass";

    connectionString = $"Host={host};Port={port};Database={db};Username={user};Password={password}";
}
else
{
    // En local usamos Testcontainers con Podman/Docker
    var postgres = new PostgreSqlBuilder()
        .WithDatabase("testdb")
        .WithUsername("testuser")
        .WithPassword("testpass")
        .Build();

    await postgres.StartAsync();
    connectionString = postgres.ConnectionString;
}

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddControllers();

var app = builder.Build();

app.MapGet("/", () => "✅ .NET Dev Service activo con PostgreSQL");
app.MapControllers();

app.Run();