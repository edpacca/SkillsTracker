using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SkillsTracker.Core.Models;

public class User
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string? Username { get; set; }

    public string? Email { get; set; }
    public DateTime CreatedAt { get; set; }

    [JsonIgnore]
    public IList<UserSkillProgress> UserSkillProgresses { get; set; } = [];
}
