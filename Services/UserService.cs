using Microsoft.EntityFrameworkCore;
using SkillsTracker.Data;
using SkillsTracker.Models;

namespace SkillsTracker.Services;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;

    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<User>> GetUsersAsync()
    {
        return await _context.Users.AsNoTracking().ToListAsync();
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        var user = await _context.Users.AsNoTracking().SingleOrDefaultAsync(u => u.Id == id);
        return user;
    }

    public async Task<User> CreateUserAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<bool> UpdateUserAsync(int id, User user)
    {
        if (id != user.Id)
            throw new ArgumentException("User ID in request does not match user object.");

        _context.Entry(user).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.Users.AnyAsync(u => u.Id == id))
                throw new KeyNotFoundException("User not found.");
            throw; // Bubble up for logging
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An error occured while updating the user.", ex);
        }
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
            throw new KeyNotFoundException("User not found.");

        _context.Users.Remove(user);

        try
        {
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException ex)
        {
            throw new InvalidOperationException("Error deleting user.", ex);
        }
    }
}
