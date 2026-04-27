using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SkillsTracker.Core.Models;

public class Skill
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string? Name { get; set; }

    public string? Description { get; set; }

    [JsonIgnore]
    public IList<TopicSkill> TopicSkills { get; set; } = [];

    [JsonIgnore]
    public IList<UserSkillProgress> UserSkillProgresses { get; set; } = [];
}
