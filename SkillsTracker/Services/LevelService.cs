using Microsoft.EntityFrameworkCore;
using SkillsTracker.Data.Repository;
using SkillsTracker.Models;

namespace SkillsTracker.Services;

public class LevelService(IRepository<Level> repository) : ILevelService
{
    public async Task<IEnumerable<Level>> GetLevelsAsync() =>
        await repository.GetAllAsync();

    public async Task<Level?> GetLevelByIdAsync(int id) =>
        await repository.GetByIdAsync(id);

    public async Task<Level> CreateLevelAsync(Level level) =>
        await repository.CreateAsync(level);

    public async Task<bool> UpdateLevelAsync(int id, Level level)
    {
        if (id != level.Id)
            throw new ArgumentException("Level ID in request does not match level object.");

        try
        {
            return await repository.UpdateAsync(level);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await repository.ExistsAsync(id))
                throw new KeyNotFoundException("Level not found.");
            throw;
        }
    }

    public async Task<bool> DeleteLevelAsync(int id) =>
        await repository.DeleteAsync(id);
}
