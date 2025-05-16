using System.ComponentModel.DataAnnotations;
using SkillsTracker.Enums;

namespace SkillsTracker.Models
{
    public class UserSkill
    {
        public int UserId { get; set; }

        [Required]
        public User? User { get; set; }

        public int SkillId { get; set; }

        [Required]
        public Skill? Skill { get; set; }

        [Required]
        public SkillStatus Status { get; set; } = SkillStatus.Pending;
    }
}
