using Microsoft.EntityFrameworkCore;
using Moq;
using SkillsTracker.Data;
using SkillsTracker.Data.Repository;
using SkillsTracker.Models;
using SkillsTracker.Services;

namespace SkillsTracker.Tests.UnitTests.Services;

public class UserServiceTests
{
    [Fact]
    public async Task GetUsersAsync_ShouldReturnUsers()
    {
        // Data
        string[] names = ["Brandon", "Alexa", "Sally", "Jon"];
        var users = names.Select(name => new User { Name = name }).ToList().AsQueryable();

        // Setup
        var mockRepo = new Mock<IRepository<User>>();
        mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(users);
        var userService = new UserService(mockRepo.Object);

        // Act
        var result = await userService.GetUsersAsync();

        // Assert
        Assert.Equal(4, result.Count());
        foreach (var name in names)
            Assert.Contains(result, u => u.Name == name);
    }

    [Fact]
    public async Task GetUserByIdAsync_ShouldReturnCorrectUser()
    {
        // Data
        var user = new User { Id = 1, Name = "Persephone" };

        // Setup
        var mockRepo = new Mock<IRepository<User>>();
        mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(user);
        var userService = new UserService(mockRepo.Object);

        // Act
        var result = await userService.GetUserByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user, result);
    }

    // [Fact]
    // public async Task UpdateUserAsync_ShouldReturnFalse_WhenIdMismatch()
    // {
    //     // Arrange
    //     var user = new User { Id = 2, Name = "Updated Eddie" };

    //     // Act & Assert
    //     await Assert.ThrowsAsync<ArgumentException>(() => _userService.UpdateUserAsync(1, user));
    // }
}
