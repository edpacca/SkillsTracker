using Microsoft.EntityFrameworkCore;
using SkillsTracker.Models;

namespace SkillsTracker.Data.Repository;

public class TopicRepository : IRepository<Topic>
{
    private readonly ApplicationDbContext _context;

    public TopicRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Topic>> GetAllAsync()
    {
        return await _context.Topics.AsNoTracking().ToListAsync();
    }

    public async Task<Topic?> GetByIdAsync(int id)
    {
        return await _context.Topics.AsNoTracking().SingleOrDefaultAsync(t => t.Id == id);
    }

    public async Task<Topic> CreateAsync(Topic topic)
    {
        _context.Topics.Add(topic);
        await _context.SaveChangesAsync();
        return topic;
    }

    public async Task<bool> UpdateAsync(Topic topic)
    {
        _context.Entry(topic).State = EntityState.Modified;
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var topic = await _context.Topics.FindAsync(id);
        if (topic == null)
            return false;

        _context.Topics.Remove(topic);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Topics.AsNoTracking().AnyAsync(t => t.Id == id);
    }
}
