using Microsoft.EntityFrameworkCore;
using SkillsTracker.Data.Repository;
using SkillsTracker.Models;
using SkillsTracker.Models.DTOs;

namespace SkillsTracker.Services;

public class UserService : IUserService
{
    private readonly IRepository<User> _repository;

    public UserService(IRepository<User> repository)
    {
        _repository = repository;
    }

    public async Task<PagedResponse<User>> GetUsersAsync(
        int page = 0,
        int size = 10,
        string sortBy = "Id",
        bool asc = true
    )
    {
        try
        {
            return await _repository.GetAllPagedAsync(page, size, sortBy, asc);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An error occurred while retrieving users.", ex);
        }
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        try
        {
            return await _repository.GetByIdAsync(id);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"An error occurred while retrieving user with ID {id}.",
                ex
            );
        }
    }

    public async Task<User> CreateUserAsync(User user)
    {
        try
        {
            return await _repository.CreateAsync(user);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An error occurred while creating the user.", ex);
        }
    }

    public async Task<bool> UpdateUserAsync(int id, User user)
    {
        if (id != user.Id)
            throw new ArgumentException("User ID in request does not match user object.");

        try
        {
            return await _repository.UpdateAsync(user);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _repository.ExistsAsync(id))
                throw new KeyNotFoundException("User not found.");
            throw; // Bubble up for logging
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An error occurred while updating the user.", ex);
        }
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        try
        {
            return await _repository.DeleteAsync(id);
        }
        catch (DbUpdateException ex)
        {
            throw new InvalidOperationException("Error deleting user.", ex);
        }
    }
}
