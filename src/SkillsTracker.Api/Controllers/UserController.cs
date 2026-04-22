using Microsoft.AspNetCore.Mvc;
using SkillsTracker.Core.Abstractions;
using SkillsTracker.Core.DTOs;
using SkillsTracker.Core.Models;

namespace SkillsTracker.Controllers;

[Route("api/users")]
[ApiController]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResponse<User>>> GetUsers(
        int page = 0,
        int size = 10,
        string sortBy = "Id",
        bool asc = true
    )
    {
        var users = await userService.GetUsersAsync(page, size, sortBy, asc);
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        var user = await userService.GetUserByIdAsync(id);
        return user == null ? NotFound() : Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<User>> CreateUser(User user)
    {
        var created = await userService.CreateUserAsync(user);
        return Ok(created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, User user)
    {
        var success = await userService.UpdateUserAsync(id, user);
        return success ? Ok() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var success = await userService.DeleteUserAsync(id);
        return success ? NoContent() : NotFound();
    }
}
