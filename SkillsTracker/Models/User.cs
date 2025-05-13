using System.ComponentModel.DataAnnotations;

namespace SkillsTracker.Models;

public class User
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string? Name { get; set; }

    // SkillsDone
    // SkillsTraining
    //SkillsPlanned
}
