using SkillsTracker.Models;
using SkillsTracker.Models.DTOs;

namespace SkillsTracker.Services;

public interface IUserService
{
    Task<PagedResponse<User>> GetUsersAsync(int page, int size, string sortBy, bool asc);

    Task<User?> GetUserByIdAsync(int id);

    Task<User> CreateUserAsync(User user);

    Task<bool> UpdateUserAsync(int id, User user);

    Task<bool> DeleteUserAsync(int id);
}
