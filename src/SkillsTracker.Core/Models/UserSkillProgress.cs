using System.Text.Json.Serialization;

namespace SkillsTracker.Core.Models;

public class UserSkillProgress
{
    public int Id { get; set; }
    public int UserId { get; set; }

    [JsonIgnore]
    public User? User { get; set; }

    public int SkillId { get; set; }

    [JsonIgnore]
    public Skill? Skill { get; set; }

    public int LevelId { get; set; }
    public Level? Level { get; set; }

    public DateTime AchievedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
