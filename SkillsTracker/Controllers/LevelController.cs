using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillsTracker.Models;
using SkillsTracker.Services;

namespace SkillsTracker.Controllers;

[Route("api/levels")]
[ApiController]
public class LevelController : ControllerBase
{
    private readonly ILevelService _levelService;

    public LevelController(ILevelService levelService)
    {
        _levelService = levelService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Level>>> GetLevels()
    {
        try
        {
            var levels = await _levelService.GetLevelsAsync();
            return Ok(levels);
        }
        catch
        {
            return Problem("Could not get all levels");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Level>> GetLevel(int id)
    {
        try
        {
            var level = await _levelService.GetLevelByIdAsync(id);
            return level == null ? NotFound("Level does not exist") : Ok(level);
        }
        catch (ArgumentNullException)
        {
            return BadRequest("Id must be valid");
        }
        catch (InvalidOperationException)
        {
            return Problem("Multiple levels found with same Id");
        }
    }

    [HttpPost]
    public async Task<ActionResult<Level>> CreateLevel(Level level)
    {
        try
        {
            var success = await _levelService.CreateLevelAsync(level);
            return Ok(success);
        }
        catch (DBConcurrencyException)
        {
            return Problem("Level exists");
        }
        catch (DbUpdateException)
        {
            return Problem("Database update failed");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateLevel(int id, Level level)
    {
        try
        {
            var success = await _levelService.UpdateLevelAsync(id, level);
            return success ? NoContent() : NotFound();
        }
        catch (ArgumentException)
        {
            return BadRequest("Level ID mismatch.");
        }
        catch (KeyNotFoundException)
        {
            return NotFound("Level does not exist.");
        }
        catch (InvalidOperationException)
        {
            return StatusCode(500, "Database update failed.");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLevel(int id)
    {
        try
        {
            var success = await _levelService.DeleteLevelAsync(id);
            return success ? NoContent() : NotFound("Level not found.");
        }
        catch (KeyNotFoundException)
        {
            return NotFound("Level does not exist.");
        }
        catch (InvalidOperationException)
        {
            return StatusCode(409, "Level deletion failed due to existing dependencies.");
        }
        catch (Exception)
        {
            return StatusCode(500, "An unexpected error occurred.");
        }
    }
}
