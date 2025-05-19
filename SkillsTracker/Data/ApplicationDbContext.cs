using Microsoft.EntityFrameworkCore;
using SkillsTracker.Models;

namespace SkillsTracker.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Topic> Topics { get; set; }

        public DbSet<Level> Levels { get; set; }

        public DbSet<UserSkill> UserSkills { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new UserSkillEntityTypeConfiguration().Configure(modelBuilder.Entity<UserSkill>());
            new TopicSkillLevelEntityTypeConfiguration().Configure(
                modelBuilder.Entity<TopicSkillLevel>()
            );
        }
    }
}
