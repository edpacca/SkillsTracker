using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillsTracker.Core.Models;

namespace SkillsTracker.Data.EntityConfiguration;

class TopicSkillEntityTypeConfiguration : IEntityTypeConfiguration<TopicSkill>
{
    public void Configure(EntityTypeBuilder<TopicSkill> builder)
    {
        builder.HasKey(ts => new { ts.TopicId, ts.SkillId });

        builder.HasOne(ts => ts.Topic).WithMany(t => t.TopicSkills).HasForeignKey(ts => ts.TopicId);

        builder.HasOne(ts => ts.Skill).WithMany(s => s.TopicSkills).HasForeignKey(ts => ts.SkillId);
    }
}
