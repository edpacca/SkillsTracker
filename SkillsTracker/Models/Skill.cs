using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SkillsTracker.Models;

public class Skill
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string? Title { get; set; }

    public string? Description { get; set; }

    [JsonIgnore]
    public IList<TopicSkillLevel> TopicSkillLevels { get; set; } = [];

    [JsonIgnore]
    public IList<UserSkill> UserSkills { get; set; } = [];
}
