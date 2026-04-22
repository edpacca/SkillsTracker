using Microsoft.Extensions.DependencyInjection;
using SkillsTracker.Core.Abstractions;

namespace SkillsTracker.Services;

public static class ServicesServiceCollectionExtensions
{
    public static IServiceCollection AddSkillsTrackerServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ISkillService, SkillService>();
        services.AddScoped<ITopicService, TopicService>();
        services.AddScoped<ILevelService, LevelService>();
        return services;
    }
}
