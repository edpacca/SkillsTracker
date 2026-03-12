using Microsoft.EntityFrameworkCore;
using SkillsTracker.Data.Repository;
using SkillsTracker.Models;

namespace SkillsTracker.Services;

public class SkillService(IRepository<Skill> repository) : ISkillService
{
    public async Task<IEnumerable<Skill>> GetSkillsAsync() =>
        await repository.GetAllAsync();

    public async Task<Skill?> GetSkillByIdAsync(int id) =>
        await repository.GetByIdAsync(id);

    public async Task<Skill> CreateSkillAsync(Skill skill) =>
        await repository.CreateAsync(skill);

    public async Task<bool> UpdateSkillAsync(int id, Skill skill)
    {
        if (id != skill.Id)
            throw new ArgumentException("Skill ID in request does not match skill object.");

        try
        {
            return await repository.UpdateAsync(skill);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await repository.ExistsAsync(id))
                throw new KeyNotFoundException("Skill not found.");
            throw;
        }
    }

    public async Task<bool> DeleteSkillAsync(int id) =>
        await repository.DeleteAsync(id);
}
