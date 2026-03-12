using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SkillsTracker.Models;

public class User
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string? Name { get; set; }
    public string? Email { get; set; }

    [JsonIgnore]
    public IList<UserSkill> UserSkills { get; set; } = [];
}
