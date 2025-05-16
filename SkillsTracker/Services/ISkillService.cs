using SkillsTracker.Models;

namespace SkillsTracker.Services;

public interface ISkillService
{
    Task<IEnumerable<Skill>> GetSkillsAsync();

    Task<Skill?> GetSkillByIdAsync(int id);

    Task<Skill> CreateSkillAsync(Skill skill);

    Task<bool> UpdateSkillAsync(int id, Skill skill);

    Task<bool> DeleteSkillAsync(int id);
}
