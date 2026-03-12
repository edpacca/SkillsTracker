using SkillsTracker.Models;
using SkillsTracker.Models.DTOs;

namespace SkillsTracker.Services;

public interface IUserService
{
    Task<PagedResponse<User>> GetUsersAsync(int page = 0, int size = 10, string sortBy = "Id", bool asc = true);

    Task<User?> GetUserByIdAsync(int id);

    Task<User> CreateUserAsync(User user);

    Task<bool> UpdateUserAsync(int id, User user);

    Task<bool> DeleteUserAsync(int id);
}
