using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillsTracker.Models;

class TopicSkillLevelEntityTypeConfiguration : IEntityTypeConfiguration<TopicSkillLevel>
{
    public void Configure(EntityTypeBuilder<TopicSkillLevel> builder)
    {
        builder.HasKey(tsl => new
        {
            tsl.TopicId,
            tsl.SkillId,
            tsl.LevelId,
        });

        builder
            .HasOne(tsl => tsl.Topic)
            .WithMany(t => t.TopicSkillLevels)
            .HasForeignKey(tsl => tsl.TopicId);

        builder
            .HasOne(tsl => tsl.Skill)
            .WithMany(s => s.TopicSkillLevels)
            .HasForeignKey(tsl => tsl.SkillId);

        builder.HasOne(tsl => tsl.Level).WithMany().HasForeignKey(tsl => tsl.LevelId);
    }
}
