using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

var builder = WebApplication.CreateBuilder(args);

var postgres = new PostgreSqlBuilder()
    //.WithImage("registry.redhat.io/rhel10/postgresql-16")
    .WithDatabase("testdb")
    .WithUsername("testuser")
    .WithPassword("testpass")
    .Build();

await postgres.StartAsync();

var connectionString = postgres.GetConnectionString();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    context.Database.EnsureCreated();

    DbInitializer.Seed(context);
}

if (app.Environment.IsDevelopment() || true)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapGet("/", () => "✅ .NET Dev Service activo con PostgreSQL");
app.MapControllers();

app.Run();