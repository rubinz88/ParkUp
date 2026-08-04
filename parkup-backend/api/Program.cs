using api.Context;
using api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var environment = builder.Environment;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddScoped<IParkingSpotRepository, ParkingSpotRepository>();
builder.Services.AddScoped<IParkingSpotService, ParkingSpotService>();

var host = builder.Configuration["DB_HOST"];
var port = builder.Configuration["DB_PORT"];
var username = builder.Configuration["DB_USER"];
var password = builder.Configuration["DB_PASSWORD"];
var dbName = builder.Configuration["DB_NAME"];

if (string.IsNullOrWhiteSpace(host) ||
    string.IsNullOrWhiteSpace(port) ||
    string.IsNullOrWhiteSpace(username) ||
    string.IsNullOrWhiteSpace(password) ||
    string.IsNullOrWhiteSpace(dbName))
{
    throw new InvalidOperationException("Database configuration is missing");
}

var connectionStringBuilder = new Npgsql.NpgsqlConnectionStringBuilder
{
    Host = host,
    Port = int.Parse(port),
    Username = username,
    Password = password,
    Database = dbName,
    IncludeErrorDetail = environment.IsDevelopment()
};

builder.Services.AddDbContext<ParkUpDbContext>(options =>
    options.UseNpgsql(connectionStringBuilder.ConnectionString));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    var db = scope.ServiceProvider.GetRequiredService<ParkUpDbContext>();

    try
    {
        await DbInitializer.Seed(db, logger);
    } catch (Exception ex)
    {
        logger.LogError("An error occured during the seeding process: " + ex);
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.UseHttpsRedirection();

app.Run();
