using SkillsTracker.Models;

namespace SkillsTracker.Services;

public interface ITopicService
{
    Task<IEnumerable<Topic>> GetTopicsAsync();

    Task<Topic?> GetTopicByIdAsync(int id);

    Task<Topic> CreateTopicAsync(Topic topic);

    Task<bool> UpdateTopicAsync(int id, Topic topic);

    Task<bool> DeleteTopicAsync(int id);
}
