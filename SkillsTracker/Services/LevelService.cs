using Microsoft.EntityFrameworkCore;
using SkillsTracker.Data.Repository;
using SkillsTracker.Models;

namespace SkillsTracker.Services;

public class LevelService : ILevelService
{
    private readonly IRepository<Level> _repository;

    public LevelService(IRepository<Level> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Level>> GetLevelsAsync()
    {
        try
        {
            return await _repository.GetAllAsync();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An error occurred while retrieving levels.", ex);
        }
    }

    public async Task<Level?> GetLevelByIdAsync(int id)
    {
        try
        {
            return await _repository.GetByIdAsync(id);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"An error occurred while retrieving level with ID {id}.",
                ex
            );
        }
    }

    public async Task<Level> CreateLevelAsync(Level level)
    {
        try
        {
            return await _repository.CreateAsync(level);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An error occurred while creating the level.", ex);
        }
    }

    public async Task<bool> UpdateLevelAsync(int id, Level level)
    {
        if (id != level.Id)
            throw new ArgumentException("Level ID in request does not match level object.");

        try
        {
            return await _repository.UpdateAsync(level);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _repository.ExistsAsync(id))
                throw new KeyNotFoundException("Level not found.");
            throw; // Bubble up for logging
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An error occurred while updating the level.", ex);
        }
    }

    public async Task<bool> DeleteLevelAsync(int id)
    {
        try
        {
            return await _repository.DeleteAsync(id);
        }
        catch (DbUpdateException ex)
        {
            throw new InvalidOperationException("Error deleting level.", ex);
        }
    }
}
