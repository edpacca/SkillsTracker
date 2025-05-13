using System;

namespace SkillsTracker.Models;

public interface IUserService
{
    Task<IEnumerable<User>> GetUsersAsync();

    Task<User?> GetUserByIdAsync(int id);

    Task<User> CreateUserAsync(User user);

    Task<bool> UpdateUserAsync(int id, User user);

    Task<bool> DeleteUserAsync(int id);
}
