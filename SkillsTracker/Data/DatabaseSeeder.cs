using Bogus;
using Microsoft.EntityFrameworkCore;
using SkillsTracker.Models;

namespace SkillsTracker.Data
{
    public static class DatabaseSeeder
    {
        public static async Task SeedUsers(ApplicationDbContext context, int count = 50)
        {
            if (await context.Users.AnyAsync())
                return; // Prevent duplicate seeding

            var faker = new Faker<User>()
                .RuleFor(u => u.Name, f => f.Name.FullName())
                .RuleFor(u => u.Email, f => f.Internet.Email());

            var users = faker.Generate(count);

            foreach (var user in users)
            {
                Console.WriteLine(user.Name);
            }
            context.Users.AddRange(users);
            await context.SaveChangesAsync();
        }
    }
}
