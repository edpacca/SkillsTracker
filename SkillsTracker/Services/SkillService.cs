using Microsoft.EntityFrameworkCore;
using SkillsTracker.Data.Repository;
using SkillsTracker.Models;

namespace SkillsTracker.Services;

public class SkillService : ISkillService
{
    private readonly IRepository<Skill> _repository;

    public SkillService(IRepository<Skill> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Skill>> GetSkillsAsync()
    {
        try
        {
            return await _repository.GetAllAsync();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An error occurred while retrieving skills.", ex);
        }
    }

    public async Task<Skill?> GetSkillByIdAsync(int id)
    {
        try
        {
            return await _repository.GetByIdAsync(id);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"An error occurred while retrieving skill with ID {id}.",
                ex
            );
        }
    }

    public async Task<Skill> CreateSkillAsync(Skill skill)
    {
        try
        {
            return await _repository.CreateAsync(skill);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An error occurred while creating the skill.", ex);
        }
    }

    public async Task<bool> UpdateSkillAsync(int id, Skill skill)
    {
        if (id != skill.Id)
            throw new ArgumentException("Skill ID in request does not match skill object.");

        try
        {
            return await _repository.UpdateAsync(skill);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _repository.ExistsAsync(id))
                throw new KeyNotFoundException("Skill not found.");
            throw; // Bubble up for logging
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An error occurred while updating the skill.", ex);
        }
    }

    public async Task<bool> DeleteSkillAsync(int id)
    {
        try
        {
            return await _repository.DeleteAsync(id);
        }
        catch (DbUpdateException ex)
        {
            throw new InvalidOperationException("Error deleting skill.", ex);
        }
    }
}
