using Microsoft.EntityFrameworkCore;
using SkillsTracker.Core.Models;
using SkillsTracker.Data.EntityConfiguration;

namespace SkillsTracker.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Skill> Skills { get; set; }
    public DbSet<Topic> Topics { get; set; }
    public DbSet<Level> Levels { get; set; }
    public DbSet<UserSkillProgress> UserSkillProgresses { get; set; }
    public DbSet<TopicSkill> TopicSkills { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new UserSkillProgressEntityTypeConfiguration().Configure(modelBuilder.Entity<UserSkillProgress>());
        new TopicSkillEntityTypeConfiguration().Configure(modelBuilder.Entity<TopicSkill>());
    }
}
