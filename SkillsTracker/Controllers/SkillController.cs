using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillsTracker.Models;
using SkillsTracker.Services;

namespace SkillsTracker.Controllers;

[Route("skills")]
[ApiController]
public class SkillController : ControllerBase
{
    private readonly ISkillService _skillService;

    public SkillController(ISkillService skillService)
    {
        _skillService = skillService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Skill>>> GetSkills()
    {
        try
        {
            var skills = await _skillService.GetSkillsAsync();
            return Ok(skills);
        }
        catch
        {
            return Problem("Could not get all skills");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Skill>> GetSkill(int id)
    {
        try
        {
            var skill = await _skillService.GetSkillByIdAsync(id);
            return skill == null ? NotFound("Skill does not exist") : Ok(skill);
        }
        catch (ArgumentNullException)
        {
            return BadRequest("Id must be valid");
        }
        catch (InvalidOperationException)
        {
            return Problem("Multiple skills found with same Id");
        }
    }

    [HttpPost]
    public async Task<ActionResult<Skill>> CreateSkill(Skill skill)
    {
        try
        {
            var success = await _skillService.CreateSkillAsync(skill);
            return Ok(success);
        }
        catch (DBConcurrencyException)
        {
            return Problem("Skill exists");
        }
        catch (DbUpdateException)
        {
            return Problem("Database update failed");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSkill(int id, Skill skill)
    {
        try
        {
            var success = await _skillService.UpdateSkillAsync(id, skill);
            return success ? NoContent() : NotFound();
        }
        catch (ArgumentException)
        {
            return BadRequest("Skill ID mismatch.");
        }
        catch (KeyNotFoundException)
        {
            return NotFound("Skill does not exist.");
        }
        catch (InvalidOperationException)
        {
            return StatusCode(500, "Database update failed.");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSkill(int id)
    {
        try
        {
            var success = await _skillService.DeleteSkillAsync(id);
            return success ? NoContent() : NotFound("Skill not found.");
        }
        catch (KeyNotFoundException)
        {
            return NotFound("Skill does not exist.");
        }
        catch (InvalidOperationException)
        {
            return StatusCode(409, "Skill deletion failed due to existing dependencies.");
        }
        catch (Exception)
        {
            return StatusCode(500, "An unexpected error occurred.");
        }
    }
}
