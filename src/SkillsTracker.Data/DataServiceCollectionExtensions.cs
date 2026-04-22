using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SkillsTracker.Core.Abstractions;
using SkillsTracker.Core.Models;
using SkillsTracker.Data.Repository;

namespace SkillsTracker.Data;

public static class DataServiceCollectionExtensions
{
    public static IServiceCollection AddSkillsTrackerData(
        this IServiceCollection services,
        string? connectionString)
    {
        services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));
        services.AddScoped<IPagedRepository<User>, UserRepository>();
        services.AddScoped<IRepository<Skill>, SkillRepository>();
        services.AddScoped<IRepository<Topic>, TopicRepository>();
        services.AddScoped<IRepository<Level>, LevelRepository>();
        return services;
    }
}
