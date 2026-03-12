using Microsoft.AspNetCore.Mvc;
using SkillsTracker.Models;
using SkillsTracker.Services;

namespace SkillsTracker.Controllers;

[Route("api/skills")]
[ApiController]
public class SkillController(ISkillService skillService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Skill>>> GetSkills()
    {
        var skills = await skillService.GetSkillsAsync();
        return Ok(skills);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Skill>> GetSkill(int id)
    {
        var skill = await skillService.GetSkillByIdAsync(id);
        return skill == null ? NotFound() : Ok(skill);
    }

    [HttpPost]
    public async Task<ActionResult<Skill>> CreateSkill(Skill skill)
    {
        var created = await skillService.CreateSkillAsync(skill);
        return Ok(created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSkill(int id, Skill skill)
    {
        var success = await skillService.UpdateSkillAsync(id, skill);
        return success ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSkill(int id)
    {
        var success = await skillService.DeleteSkillAsync(id);
        return success ? NoContent() : NotFound();
    }
}
