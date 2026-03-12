using Microsoft.EntityFrameworkCore;
using SkillsTracker.Data.Repository;
using SkillsTracker.Models;
using SkillsTracker.Models.DTOs;

namespace SkillsTracker.Services;

public class UserService(IPagedRepository<User> repository) : IUserService
{
    public async Task<PagedResponse<User>> GetUsersAsync(
        int page = 0,
        int size = 10,
        string sortBy = "Id",
        bool asc = true
    ) => await repository.GetAllPagedAsync(page, size, sortBy, asc);

    public async Task<User?> GetUserByIdAsync(int id) =>
        await repository.GetByIdAsync(id);

    public async Task<User> CreateUserAsync(User user) =>
        await repository.CreateAsync(user);

    public async Task<bool> UpdateUserAsync(int id, User user)
    {
        if (id != user.Id)
            throw new ArgumentException("User ID in request does not match user object.");

        try
        {
            return await repository.UpdateAsync(user);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await repository.ExistsAsync(id))
                throw new KeyNotFoundException("User not found.");
            throw;
        }
    }

    public async Task<bool> DeleteUserAsync(int id) =>
        await repository.DeleteAsync(id);
}
