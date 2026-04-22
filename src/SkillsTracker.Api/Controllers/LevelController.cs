using Microsoft.AspNetCore.Mvc;
using SkillsTracker.Core.Abstractions;
using SkillsTracker.Core.Models;

namespace SkillsTracker.Controllers;

[Route("api/levels")]
[ApiController]
public class LevelController(ILevelService levelService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Level>>> GetLevels()
    {
        var levels = await levelService.GetLevelsAsync();
        return Ok(levels);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Level>> GetLevel(int id)
    {
        var level = await levelService.GetLevelByIdAsync(id);
        return level == null ? NotFound() : Ok(level);
    }

    [HttpPost]
    public async Task<ActionResult<Level>> CreateLevel(Level level)
    {
        var created = await levelService.CreateLevelAsync(level);
        return Ok(created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateLevel(int id, Level level)
    {
        var success = await levelService.UpdateLevelAsync(id, level);
        return success ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLevel(int id)
    {
        var success = await levelService.DeleteLevelAsync(id);
        return success ? NoContent() : NotFound();
    }
}
