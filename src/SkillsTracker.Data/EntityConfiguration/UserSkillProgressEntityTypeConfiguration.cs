using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillsTracker.Core.Models;

namespace SkillsTracker.Data.EntityConfiguration;

class UserSkillProgressEntityTypeConfiguration : IEntityTypeConfiguration<UserSkillProgress>
{
    public void Configure(EntityTypeBuilder<UserSkillProgress> builder)
    {
        builder.HasKey(usp => usp.Id);

        builder.HasIndex(usp => new { usp.UserId, usp.SkillId }).IsUnique();

        builder.HasOne(usp => usp.User).WithMany(u => u.UserSkillProgresses).HasForeignKey(usp => usp.UserId);

        builder.HasOne(usp => usp.Skill).WithMany(s => s.UserSkillProgresses).HasForeignKey(usp => usp.SkillId);

        builder.HasOne(usp => usp.Level).WithMany().HasForeignKey(usp => usp.LevelId);
    }
}
