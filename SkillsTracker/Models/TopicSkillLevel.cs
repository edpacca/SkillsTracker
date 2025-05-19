using System.ComponentModel.DataAnnotations;
using SkillsTracker.Enums;

namespace SkillsTracker.Models;

public class TopicSkillLevel
{
    public int TopicId { get; set; }

    [Required]
    public Topic? Topic { get; set; }

    public int SkillId { get; set; }

    [Required]
    public Skill? Skill { get; set; }

    public int LevelId { get; set; }

    [Required]
    public Level? Level { get; set; }
}
