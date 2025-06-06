using SkillsTracker.Models.DTOs;

namespace SkillsTracker.Data.Repository;

public interface IRepository<T>
    where T : class
{
    Task<PagedResponse<T>> GetAllPagedAsync(int page, int size, string sortBy, bool asc);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task<T> CreateAsync(T entity);
    Task<bool> UpdateAsync(T entity);
    Task<bool> DeleteAsync(int id);

    Task<bool> ExistsAsync(int id);
}
