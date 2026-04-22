using Avalonia;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SkillsTracker.Data;
using SkillsTracker.Desktop.ViewModels;
using SkillsTracker.Services;

namespace SkillsTracker.Desktop;

sealed class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var services = new ServiceCollection();
        var cs = config.GetConnectionString("PostgresConnection")!;
        services.AddSkillsTrackerData(cs);
        services.AddSkillsTrackerServices();
        services.AddSingleton<MainWindowViewModel>();
        services.AddTransient<UsersViewModel>();

        App.Services = services.BuildServiceProvider();

        using (var scope = App.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            DatabaseSeeder.SeedUsers(db).GetAwaiter().GetResult();
        }

        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
#if DEBUG
            .WithDeveloperTools()
#endif
            .WithInterFont()
            .LogToTrace();
}
