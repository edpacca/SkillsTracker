using Microsoft.EntityFrameworkCore;
using MudBlazor;
using SkillsTracker.Models;
using SkillsTracker.Models.DTOs;

namespace SkillsTracker.Data.Repository;

public class UserRepository : IRepository<User>
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users.AsNoTracking().ToListAsync();
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users.AsNoTracking().SingleOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User> CreateAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<bool> UpdateAsync(User user)
    {
        _context.Entry(user).State = EntityState.Modified;
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
            return false;

        _context.Users.Remove(user);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Users.AsNoTracking().AnyAsync(u => u.Id == id);
    }

    public async Task<PagedResponse<User>> GetAllPagedAsync(
        int page,
        int size,
        string sortBy,
        bool asc = true
    )
    {
        var query = _context.Users.AsQueryable();

        if (!string.IsNullOrEmpty(sortBy))
        {
            query = asc ? query.OrderBy(u => u.Id) : query.OrderByDescending(u => u.Id);
        }

        var totalCount = await query.CountAsync();
        var users = await query.Skip(page * size).Take(size).ToListAsync();

        return new PagedResponse<User> { Data = users, TotalCount = totalCount };
    }
}
