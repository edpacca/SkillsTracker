using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillsTracker.Models;
using SkillsTracker.Services;

namespace SkillsTracker.Controllers;

[Route("users")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        try
        {
            var users = await _userService.GetUsersAsync();
            return Ok(users);
        }
        catch
        {
            return Problem("Could not get all users");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        try
        {
            var user = await _userService.GetUserByIdAsync(id);
            return user == null ? NotFound("User does not exist") : Ok(user);
        }
        catch (ArgumentNullException)
        {
            return BadRequest("Id must be valid");
        }
        catch (InvalidOperationException)
        {
            return Problem("Multiple users found with same Id");
        }
    }

    [HttpPost]
    public async Task<ActionResult<User>> CreateUser(User user)
    {
        try
        {
            var success = await _userService.CreateUserAsync(user);
            return Ok(success);
        }
        catch (DBConcurrencyException)
        {
            return Problem("User exists");
        }
        catch (DbUpdateException)
        {
            return Problem("Database update failed");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, User user)
    {
        try
        {
            var success = await _userService.UpdateUserAsync(id, user);
            return success ? NoContent() : NotFound();
        }
        catch (ArgumentException)
        {
            return BadRequest("User ID mismatch.");
        }
        catch (KeyNotFoundException)
        {
            return NotFound("User does not exist.");
        }
        catch (InvalidOperationException)
        {
            return StatusCode(500, "Database update failed.");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        try
        {
            var success = await _userService.DeleteUserAsync(id);
            return success ? NoContent() : NotFound("User not found.");
        }
        catch (KeyNotFoundException)
        {
            return NotFound("User does not exist.");
        }
        catch (InvalidOperationException)
        {
            return StatusCode(409, "User deletion failed due to existing dependencies.");
        }
        catch (Exception)
        {
            return StatusCode(500, "An unexpected error occurred.");
        }
    }
}
