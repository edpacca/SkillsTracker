using Microsoft.EntityFrameworkCore;
using SkillsTracker.Models;

namespace SkillsTracker.Data.Repository;

public class LevelRepository : IRepository<Level>
{
    private readonly ApplicationDbContext _context;

    public LevelRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Level>> GetAllAsync()
    {
        return await _context.Levels.AsNoTracking().ToListAsync();
    }

    public async Task<Level?> GetByIdAsync(int id)
    {
        return await _context.Levels.AsNoTracking().SingleOrDefaultAsync(l => l.Id == id);
    }

    public async Task<Level> CreateAsync(Level level)
    {
        _context.Levels.Add(level);
        await _context.SaveChangesAsync();
        return level;
    }

    public async Task<bool> UpdateAsync(Level level)
    {
        _context.Entry(level).State = EntityState.Modified;
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var level = await _context.Levels.FindAsync(id);
        if (level == null)
            return false;

        _context.Levels.Remove(level);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Levels.AsNoTracking().AnyAsync(l => l.Id == id);
    }
}
