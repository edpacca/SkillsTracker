using System.ComponentModel.DataAnnotations;

namespace SkillsTracker.Core.Models;

public class Topic
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string? Name { get; set; }

    public string? Description { get; set; }

    public IList<TopicSkill> TopicSkills { get; set; } = [];
}
