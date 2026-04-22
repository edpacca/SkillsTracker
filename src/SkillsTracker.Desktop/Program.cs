using Avalonia;
using Microsoft.EntityFrameworkCore;
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

        try
        {
            using var scope = App.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            db.Database.Migrate();
            // DatabaseSeeder.SeedUsers(db).GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[Seeder] DB unavailable, skipping seed: {ex.Message}");
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