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

    public static async Task SeedSkillsAndTopics(ApplicationDbContext context)
    {
        if (await context.Topics.AnyAsync())
            return;

        var levelNames = new[] { ("Beginner", 1), ("Intermediate", 2), ("Advanced", 3) };

        var topicNames = new[]
        {
            "Frontend Development",
            "Backend Development",
            "DevOps",
            "Data Engineering",
            "Cloud Computing",
        };

        var topics = topicNames.Select(name => new Topic { Name = name }).ToList();
        context.Topics.AddRange(topics);
        await context.SaveChangesAsync();

        var levels = topics
            .SelectMany(t => levelNames.Select(l => new Level
            {
                Name = l.Item1,
                Value = l.Item2,
                TopicId = t.Id,
            }))
            .ToList();
        context.Levels.AddRange(levels);
        await context.SaveChangesAsync();

        Level LevelFor(Topic t, string name) =>
            levels.First(l => l.TopicId == t.Id && l.Name == name);

        var frontend = topics[0];
        var backend = topics[1];
        var devops = topics[2];
        var dataEng = topics[3];
        var cloud = topics[4];

        var skillDefs = new[]
        {
            new { Title = "JavaScript",      Description = "Dynamic scripting language for web development" },
            new { Title = "TypeScript",      Description = "Typed superset of JavaScript for large-scale apps" },
            new { Title = "React",           Description = "Component-based UI library by Meta" },
            new { Title = "CSS & Styling",   Description = "Cascading stylesheets and modern layout techniques" },
            new { Title = "Python",          Description = "Versatile language used in web, data, and scripting" },
            new { Title = "SQL",             Description = "Structured query language for relational databases" },
            new { Title = "REST APIs",       Description = "Designing and consuming HTTP-based web services" },
            new { Title = "Git",             Description = "Distributed version control system" },
            new { Title = "Docker",          Description = "Container platform for packaging and running applications" },
            new { Title = "Kubernetes",      Description = "Container orchestration for scaling and managing workloads" },
            new { Title = "CI/CD",           Description = "Continuous integration and delivery pipelines" },
            new { Title = "Linux",           Description = "Command-line proficiency and shell scripting" },
            new { Title = "Terraform",       Description = "Infrastructure-as-code tool for cloud provisioning" },
            new { Title = "PostgreSQL",      Description = "Advanced open-source relational database" },
            new { Title = "System Design",   Description = "Designing scalable, reliable distributed systems" },
        };

        var skills = skillDefs
            .Select(s => new Skill { Title = s.Title, Description = s.Description })
            .ToList();
        context.Skills.AddRange(skills);
        await context.SaveChangesAsync();

        Skill S(string title) => skills.First(s => s.Title == title);

        // Each entry: (topic, skill, level name)
        var assignments = new (Topic Topic, Skill Skill, string Level)[]
        {
            // Frontend
            (frontend, S("JavaScript"),    "Beginner"),
            (frontend, S("JavaScript"),    "Intermediate"),
            (frontend, S("JavaScript"),    "Advanced"),
            (frontend, S("TypeScript"),    "Intermediate"),
            (frontend, S("TypeScript"),    "Advanced"),
            (frontend, S("React"),         "Beginner"),
            (frontend, S("React"),         "Intermediate"),
            (frontend, S("React"),         "Advanced"),
            (frontend, S("CSS & Styling"), "Beginner"),
            (frontend, S("CSS & Styling"), "Intermediate"),
            (frontend, S("Git"),           "Beginner"),

            // Backend
            (backend, S("Python"),       "Beginner"),
            (backend, S("Python"),       "Intermediate"),
            (backend, S("Python"),       "Advanced"),
            (backend, S("SQL"),          "Beginner"),
            (backend, S("SQL"),          "Intermediate"),
            (backend, S("REST APIs"),    "Beginner"),
            (backend, S("REST APIs"),    "Intermediate"),
            (backend, S("REST APIs"),    "Advanced"),
            (backend, S("Git"),          "Beginner"),
            (backend, S("Docker"),       "Intermediate"),
            (backend, S("PostgreSQL"),   "Intermediate"),
            (backend, S("PostgreSQL"),   "Advanced"),
            (backend, S("System Design"),"Advanced"),

            // DevOps
            (devops, S("Docker"),      "Beginner"),
            (devops, S("Docker"),      "Intermediate"),
            (devops, S("Docker"),      "Advanced"),
            (devops, S("Kubernetes"),  "Intermediate"),
            (devops, S("Kubernetes"),  "Advanced"),
            (devops, S("CI/CD"),       "Beginner"),
            (devops, S("CI/CD"),       "Intermediate"),
            (devops, S("CI/CD"),       "Advanced"),
            (devops, S("Linux"),       "Beginner"),
            (devops, S("Linux"),       "Intermediate"),
            (devops, S("Git"),         "Intermediate"),
            (devops, S("Terraform"),   "Intermediate"),
            (devops, S("Terraform"),   "Advanced"),

            // Data Engineering
            (dataEng, S("Python"),      "Intermediate"),
            (dataEng, S("Python"),      "Advanced"),
            (dataEng, S("SQL"),         "Intermediate"),
            (dataEng, S("SQL"),         "Advanced"),
            (dataEng, S("PostgreSQL"),  "Beginner"),
            (dataEng, S("Linux"),       "Intermediate"),
            (dataEng, S("Docker"),      "Intermediate"),

            // Cloud
            (cloud, S("Terraform"),    "Beginner"),
            (cloud, S("Terraform"),    "Intermediate"),
            (cloud, S("Terraform"),    "Advanced"),
            (cloud, S("Docker"),       "Intermediate"),
            (cloud, S("Kubernetes"),   "Advanced"),
            (cloud, S("CI/CD"),        "Advanced"),
            (cloud, S("Linux"),        "Intermediate"),
            (cloud, S("System Design"),"Advanced"),
        };

        var topicSkillLevels = assignments
            .Select(a => new TopicSkillLevel
            {
                TopicId = a.Topic.Id,
                SkillId = a.Skill.Id,
                LevelId = LevelFor(a.Topic, a.Level).Id,
            })
            .ToList();

        context.TopicSkillLevels.AddRange(topicSkillLevels);
        await context.SaveChangesAsync();
    }
}
