using Bogus;
using Microsoft.EntityFrameworkCore;
using SkillsTracker.Core.Models;

namespace SkillsTracker.Data;

public static class DatabaseSeeder
{
    public static async Task SeedUsers(ApplicationDbContext context, int count = 50)
    {
        if (await context.Users.AnyAsync())
            return;

        var faker = new Faker<User>()
            .RuleFor(u => u.Username, f => f.Internet.UserName())
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.CreatedAt, f => f.Date.Past(2).ToUniversalTime());

        var users = faker.Generate(count);
        context.Users.AddRange(users);
        await context.SaveChangesAsync();
    }

    public static async Task SeedSkillsAndTopics(ApplicationDbContext context)
    {
        if (await context.Topics.AnyAsync())
            return;

        var levelDefs = new[] { ("Beginner", 1), ("Intermediate", 2), ("Advanced", 3) };
        var levels = levelDefs.Select(l => new Level { Name = l.Item1, SortOrder = l.Item2 }).ToList();
        context.Levels.AddRange(levels);
        await context.SaveChangesAsync();

        var skillDefs = new Dictionary<string, string>
        {
            ["JavaScript"]    = "Dynamic scripting language for web development",
            ["TypeScript"]    = "Typed superset of JavaScript for large-scale apps",
            ["React"]         = "Component-based UI library by Meta",
            ["CSS & Styling"] = "Cascading stylesheets and modern layout techniques",
            ["Python"]        = "Versatile language used in web, data, and scripting",
            ["SQL"]           = "Structured query language for relational databases",
            ["REST APIs"]     = "Designing and consuming HTTP-based web services",
            ["Git"]           = "Distributed version control system",
            ["Docker"]        = "Container platform for packaging and running applications",
            ["Kubernetes"]    = "Container orchestration for scaling and managing workloads",
            ["CI/CD"]         = "Continuous integration and delivery pipelines",
            ["Linux"]         = "Command-line proficiency and shell scripting",
            ["Terraform"]     = "Infrastructure-as-code tool for cloud provisioning",
            ["PostgreSQL"]    = "Advanced open-source relational database",
            ["System Design"] = "Designing scalable, reliable distributed systems",
        };

        var topicDefs = new[]
        {
            Topic("Frontend Development",  "Building user interfaces and client-side web applications",
                "JavaScript", "TypeScript", "React", "CSS & Styling", "Git"),

            Topic("Backend Development",   "Server-side logic, APIs, and data persistence",
                "Python", "SQL", "REST APIs", "Git", "Docker", "PostgreSQL", "System Design"),

            Topic("DevOps",                "Infrastructure automation, CI/CD, and container orchestration",
                "Docker", "Kubernetes", "CI/CD", "Linux", "Git", "Terraform"),

            Topic("Data Engineering",      "Data pipelines, storage, and processing at scale",
                "Python", "SQL", "PostgreSQL", "Linux", "Docker"),

            Topic("Cloud Computing",       "Provisioning and managing cloud infrastructure and services",
                "Terraform", "Docker", "Kubernetes", "CI/CD", "Linux", "System Design"),
        };

        var skills = skillDefs.ToDictionary(
            kv => kv.Key,
            kv => new Skill { Name = kv.Key, Description = kv.Value });

        context.Skills.AddRange(skills.Values);
        await context.SaveChangesAsync();

        var topics = topicDefs.Select(td => td.Entity).ToList();
        context.Topics.AddRange(topics);
        await context.SaveChangesAsync();

        var topicSkills = topicDefs.SelectMany(td =>
            td.SkillNames.Select(name => new TopicSkill
            {
                TopicId = td.Entity.Id,
                SkillId = skills[name].Id,
            }));

        context.TopicSkills.AddRange(topicSkills);
        await context.SaveChangesAsync();
    }

    private static (Topic Entity, string[] SkillNames) Topic(string name, string description, params string[] skillNames) =>
        (new Topic { Name = name, Description = description }, skillNames);
}
