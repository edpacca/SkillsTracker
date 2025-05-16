using System.ComponentModel.DataAnnotations;

namespace SkillsTracker.Models;

public class Topic
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string? Name { get; set; }

    public IList<Level> Levels { get; set; } = [];
}
