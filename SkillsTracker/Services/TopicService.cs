using Microsoft.EntityFrameworkCore;
using SkillsTracker.Data.Repository;
using SkillsTracker.Models;

namespace SkillsTracker.Services;

public class TopicService : ITopicService
{
    private readonly IRepository<Topic> _repository;

    public TopicService(IRepository<Topic> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Topic>> GetTopicsAsync()
    {
        try
        {
            return await _repository.GetAllAsync();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An error occurred while retrieving topics.", ex);
        }
    }

    public async Task<Topic?> GetTopicByIdAsync(int id)
    {
        try
        {
            return await _repository.GetByIdAsync(id);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"An error occurred while retrieving topic with ID {id}.",
                ex
            );
        }
    }

    public async Task<Topic> CreateTopicAsync(Topic topic)
    {
        try
        {
            return await _repository.CreateAsync(topic);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An error occurred while creating the topic.", ex);
        }
    }

    public async Task<bool> UpdateTopicAsync(int id, Topic topic)
    {
        if (id != topic.Id)
            throw new ArgumentException("Topic ID in request does not match topic object.");

        try
        {
            return await _repository.UpdateAsync(topic);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _repository.ExistsAsync(id))
                throw new KeyNotFoundException("Topic not found.");
            throw; // Bubble up for logging
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An error occurred while updating the topic.", ex);
        }
    }

    public async Task<bool> DeleteTopicAsync(int id)
    {
        try
        {
            return await _repository.DeleteAsync(id);
        }
        catch (DbUpdateException ex)
        {
            throw new InvalidOperationException("Error deleting topic.", ex);
        }
    }
}
