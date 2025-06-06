using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillsTracker.Models;
using SkillsTracker.Services;

namespace SkillsTracker.Controllers;

[Route("api/topics")]
[ApiController]
public class TopicController : ControllerBase
{
    private readonly ITopicService _topicService;

    public TopicController(ITopicService topicService)
    {
        _topicService = topicService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Topic>>> GetTopics()
    {
        try
        {
            var topics = await _topicService.GetTopicsAsync();
            return Ok(topics);
        }
        catch
        {
            return Problem("Could not get all topics");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Topic>> GetTopic(int id)
    {
        try
        {
            var topic = await _topicService.GetTopicByIdAsync(id);
            return topic == null ? NotFound("Topic does not exist") : Ok(topic);
        }
        catch (ArgumentNullException)
        {
            return BadRequest("Id must be valid");
        }
        catch (InvalidOperationException)
        {
            return Problem("Multiple topics found with same Id");
        }
    }

    [HttpPost]
    public async Task<ActionResult<Topic>> CreateTopic(Topic topic)
    {
        try
        {
            var success = await _topicService.CreateTopicAsync(topic);
            return Ok(success);
        }
        catch (DBConcurrencyException)
        {
            return Problem("Topic exists");
        }
        catch (DbUpdateException)
        {
            return Problem("Database update failed");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTopic(int id, Topic topic)
    {
        try
        {
            var success = await _topicService.UpdateTopicAsync(id, topic);
            return success ? NoContent() : NotFound();
        }
        catch (ArgumentException)
        {
            return BadRequest("Topic ID mismatch.");
        }
        catch (KeyNotFoundException)
        {
            return NotFound("Topic does not exist.");
        }
        catch (InvalidOperationException)
        {
            return StatusCode(500, "Database update failed.");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTopic(int id)
    {
        try
        {
            var success = await _topicService.DeleteTopicAsync(id);
            return success ? NoContent() : NotFound("Topic not found.");
        }
        catch (KeyNotFoundException)
        {
            return NotFound("Topic does not exist.");
        }
        catch (InvalidOperationException)
        {
            return StatusCode(409, "Topic deletion failed due to existing dependencies.");
        }
        catch (Exception)
        {
            return StatusCode(500, "An unexpected error occurred.");
        }
    }
}
