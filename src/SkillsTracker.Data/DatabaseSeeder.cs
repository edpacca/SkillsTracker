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

        var topicDefs = new[]
        {
            ("Frontend Development",  "Building user interfaces and client-side web applications"),
            ("Backend Development",   "Server-side logic, APIs, and data persistence"),
            ("DevOps",                "Infrastructure automation, CI/CD, and container orchestration"),
            ("Data Engineering",      "Data pipelines, storage, and processing at scale"),
            ("Cloud Computing",       "Provisioning and managing cloud infrastructure and services"),
        };

        var topics = topicDefs.Select(t => new Topic { Name = t.Item1, Description = t.Item2 }).ToList();
        context.Topics.AddRange(topics);
        await context.SaveChangesAsync();

        var skillDefs = new[]
        {
            ("JavaScript",    "Dynamic scripting language for web development"),
            ("TypeScript",    "Typed superset of JavaScript for large-scale apps"),
            ("React",         "Component-based UI library by Meta"),
            ("CSS & Styling", "Cascading stylesheets and modern layout techniques"),
            ("Python",        "Versatile language used in web, data, and scripting"),
            ("SQL",           "Structured query language for relational databases"),
            ("REST APIs",     "Designing and consuming HTTP-based web services"),
            ("Git",           "Distributed version control system"),
            ("Docker",        "Container platform for packaging and running applications"),
            ("Kubernetes",    "Container orchestration for scaling and managing workloads"),
            ("CI/CD",         "Continuous integration and delivery pipelines"),
            ("Linux",         "Command-line proficiency and shell scripting"),
            ("Terraform",     "Infrastructure-as-code tool for cloud provisioning"),
            ("PostgreSQL",    "Advanced open-source relational database"),
            ("System Design", "Designing scalable, reliable distributed systems"),
        };

        var skills = skillDefs.Select(s => new Skill { Name = s.Item1, Description = s.Item2 }).ToList();
        context.Skills.AddRange(skills);
        await context.SaveChangesAsync();

        Topic T(string name) => topics.First(t => t.Name == name);
        Skill S(string name) => skills.First(s => s.Name == name);

        var topicSkills = new (Topic Topic, Skill Skill)[]
        {
            // Frontend
            (T("Frontend Development"), S("JavaScript")),
            (T("Frontend Development"), S("TypeScript")),
            (T("Frontend Development"), S("React")),
            (T("Frontend Development"), S("CSS & Styling")),
            (T("Frontend Development"), S("Git")),

            // Backend
            (T("Backend Development"), S("Python")),
            (T("Backend Development"), S("SQL")),
            (T("Backend Development"), S("REST APIs")),
            (T("Backend Development"), S("Git")),
            (T("Backend Development"), S("Docker")),
            (T("Backend Development"), S("PostgreSQL")),
            (T("Backend Development"), S("System Design")),

            // DevOps
            (T("DevOps"), S("Docker")),
            (T("DevOps"), S("Kubernetes")),
            (T("DevOps"), S("CI/CD")),
            (T("DevOps"), S("Linux")),
            (T("DevOps"), S("Git")),
            (T("DevOps"), S("Terraform")),

            // Data Engineering
            (T("Data Engineering"), S("Python")),
            (T("Data Engineering"), S("SQL")),
            (T("Data Engineering"), S("PostgreSQL")),
            (T("Data Engineering"), S("Linux")),
            (T("Data Engineering"), S("Docker")),

            // Cloud
            (T("Cloud Computing"), S("Terraform")),
            (T("Cloud Computing"), S("Docker")),
            (T("Cloud Computing"), S("Kubernetes")),
            (T("Cloud Computing"), S("CI/CD")),
            (T("Cloud Computing"), S("Linux")),
            (T("Cloud Computing"), S("System Design")),
        };

        context.TopicSkills.AddRange(topicSkills.Select(ts => new TopicSkill
        {
            TopicId = ts.Topic.Id,
            SkillId = ts.Skill.Id,
        }));
        await context.SaveChangesAsync();
    }
}
