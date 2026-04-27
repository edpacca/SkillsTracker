using System.ComponentModel.DataAnnotations;

namespace SkillsTracker.Core.Models;

public class Level
{
    public int Id { get; set; }

    [Required]
    [MaxLength(36)]
    public string? Name { get; set; }

    public int SortOrder { get; set; }
}
