using System.Text.Json.Serialization;

namespace SkillsTracker.Core.Models;

public class TopicSkill
{
    public int TopicId { get; set; }

    [JsonIgnore]
    public Topic? Topic { get; set; }

    public int SkillId { get; set; }

    [JsonIgnore]
    public Skill? Skill { get; set; }
}
