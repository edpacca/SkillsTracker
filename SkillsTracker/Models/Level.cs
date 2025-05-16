using System.ComponentModel.DataAnnotations;

namespace SkillsTracker.Models;

public class Level
{
    public int Id { get; set; }

    [Required]
    [MaxLength(36)]
    public string? Name { get; set; }

    public int? Value { get; set; }

    public IList<Skill> Skills { get; set; } = [];
}
