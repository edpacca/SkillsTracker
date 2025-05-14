using Microsoft.EntityFrameworkCore;
using SkillsTracker.Models;

namespace SkillsTracker.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        // public DbSet<Skill> Skills { get; set; }
        // public DbSet<Topic> Topics { get; set; }
    }
}
