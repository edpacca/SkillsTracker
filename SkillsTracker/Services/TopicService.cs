using Microsoft.EntityFrameworkCore;
using SkillsTracker.Data.Repository;
using SkillsTracker.Models;

namespace SkillsTracker.Services;

public class TopicService(IRepository<Topic> repository) : ITopicService
{
    public async Task<IEnumerable<Topic>> GetTopicsAsync() =>
        await repository.GetAllAsync();

    public async Task<Topic?> GetTopicByIdAsync(int id) =>
        await repository.GetByIdAsync(id);

    public async Task<Topic> CreateTopicAsync(Topic topic) =>
        await repository.CreateAsync(topic);

    public async Task<bool> UpdateTopicAsync(int id, Topic topic)
    {
        if (id != topic.Id)
            throw new ArgumentException("Topic ID in request does not match topic object.");

        try
        {
            return await repository.UpdateAsync(topic);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await repository.ExistsAsync(id))
                throw new KeyNotFoundException("Topic not found.");
            throw;
        }
    }

    public async Task<bool> DeleteTopicAsync(int id) =>
        await repository.DeleteAsync(id);
}
