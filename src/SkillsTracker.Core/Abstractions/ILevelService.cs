using SkillsTracker.Core.Models;

namespace SkillsTracker.Core.Abstractions;

public interface ILevelService
{
    Task<IEnumerable<Level>> GetLevelsAsync();

    Task<Level?> GetLevelByIdAsync(int id);

    Task<Level> CreateLevelAsync(Level level);

    Task<bool> UpdateLevelAsync(int id, Level level);

    Task<bool> DeleteLevelAsync(int id);
}
