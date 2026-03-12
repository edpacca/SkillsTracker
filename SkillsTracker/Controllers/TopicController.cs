using Microsoft.AspNetCore.Mvc;
using SkillsTracker.Models;
using SkillsTracker.Services;

namespace SkillsTracker.Controllers;

[Route("api/topics")]
[ApiController]
public class TopicController(ITopicService topicService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Topic>>> GetTopics()
    {
        var topics = await topicService.GetTopicsAsync();
        return Ok(topics);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Topic>> GetTopic(int id)
    {
        var topic = await topicService.GetTopicByIdAsync(id);
        return topic == null ? NotFound() : Ok(topic);
    }

    [HttpPost]
    public async Task<ActionResult<Topic>> CreateTopic(Topic topic)
    {
        var created = await topicService.CreateTopicAsync(topic);
        return Ok(created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTopic(int id, Topic topic)
    {
        var success = await topicService.UpdateTopicAsync(id, topic);
        return success ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTopic(int id)
    {
        var success = await topicService.DeleteTopicAsync(id);
        return success ? NoContent() : NotFound();
    }
}
