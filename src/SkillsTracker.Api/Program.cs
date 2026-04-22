using Scalar.AspNetCore;
using SkillsTracker.Data;
using SkillsTracker.Middleware;
using SkillsTracker.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("PostgresConnection");

builder.Services.AddSkillsTrackerData(connectionString!);
builder.Services.AddSkillsTrackerServices();

builder.Services.AddControllers();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseHsts();
    app.MapOpenApi();
    app.MapScalarApiReference(); // endpoint at: /scalar/v1
}

app.UseHttpsRedirection();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
