using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillsTracker.Models;

class UserSkillEntityTypeConfiguration : IEntityTypeConfiguration<UserSkill>
{
    public void Configure(EntityTypeBuilder<UserSkill> builder)
    {
        builder.HasKey(us => new { us.UserId, us.SkillId }); // Composite Key

        builder.HasOne(us => us.User).WithMany(u => u.UserSkills).HasForeignKey(us => us.UserId);

        builder.HasOne(us => us.Skill).WithMany(s => s.UserSkills).HasForeignKey(us => us.SkillId);
    }
}
