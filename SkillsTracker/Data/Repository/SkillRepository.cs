using Microsoft.EntityFrameworkCore;
using SkillsTracker.Models;
using SkillsTracker.Models.DTOs;

namespace SkillsTracker.Data.Repository;

public class SkillRepository : IRepository<Skill>
{
    private readonly ApplicationDbContext _context;

    public SkillRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Skill>> GetAllAsync()
    {
        return await _context.Skills.AsNoTracking().ToListAsync();
    }

    public async Task<Skill?> GetByIdAsync(int id)
    {
        return await _context.Skills.AsNoTracking().SingleOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Skill> CreateAsync(Skill skill)
    {
        _context.Skills.Add(skill);
        await _context.SaveChangesAsync();
        return skill;
    }

    public async Task<bool> UpdateAsync(Skill skill)
    {
        _context.Entry(skill).State = EntityState.Modified;
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var skill = await _context.Skills.FindAsync(id);
        if (skill == null)
            return false;

        _context.Skills.Remove(skill);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Skills.AsNoTracking().AnyAsync(s => s.Id == id);
    }

    public Task<PagedResponse<Skill>> GetAllPagedAsync(int page, int size, string sortBy, bool asc)
    {
        throw new NotImplementedException();
    }
}
