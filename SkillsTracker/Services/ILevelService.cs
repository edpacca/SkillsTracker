using SkillsTracker.Models;

namespace SkillsTracker.Services;

public interface ILevelService
{
    Task<IEnumerable<Level>> GetLevelsAsync();

    Task<Level?> GetLevelByIdAsync(int id);

    Task<Level> CreateLevelAsync(Level level);

    Task<bool> UpdateLevelAsync(int id, Level level);

    Task<bool> DeleteLevelAsync(int id);
}
