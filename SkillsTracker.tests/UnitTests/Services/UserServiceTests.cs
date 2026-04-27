using Microsoft.EntityFrameworkCore;
using Moq;
using SkillsTracker.Core.Abstractions;
using SkillsTracker.Core.DTOs;
using SkillsTracker.Core.Models;
using SkillsTracker.Services;
using SkillsTracker.Tests.UnitTests.Fixtures;

namespace SkillsTracker.Tests.UnitTests.Services;

public class UserServiceTests : IClassFixture<UserServiceMockRepository>
{
    private readonly Mock<IPagedRepository<User>> _mockRepo;
    private readonly UserService _userService;

    public UserServiceTests(UserServiceMockRepository serviceRepoFixture)
    {
        _mockRepo = serviceRepoFixture.MockRepo;
        _userService = serviceRepoFixture.UserService;
    }

    [Fact]
    public async Task GetUsersAsync_ShouldReturnUsers()
    {
        // Data
        string[] names = ["Brandon", "Alexa", "Sally", "Jon"];
        var users = names.Select(name => new User { Username = name }).ToList();
        var pagedResponse = new PagedResponse<User>()
        {
            Data = users,
            TotalCount = 4
        };

        // Setup
        _mockRepo.Setup(r => r.GetAllPagedAsync(
            It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>()
            )).ReturnsAsync(pagedResponse);

        // Act
        var result = await _userService.GetUsersAsync();

        // Assert
        Assert.Equal(4, result.TotalCount);
        Assert.Equal(names, result.Data.Select(x => x.Username));
    }

    [Fact]
    public async Task GetUserByIdAsync_ShouldReturnCorrectUser()
    {
        // Data
        var user = new User { Id = 1, Username = "Persephone" };

        // Setup
        _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(user);
        _mockRepo.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(value: null);

        // Act
        var result = await _userService.GetUserByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user, result);

        var nullResult = await _userService.GetUserByIdAsync(2);
        Assert.Null(nullResult);
    }

    [Fact]
    public async Task CreateUserAsync_ShouldReturnCreatedUser()
    {
        // Arrange
        var user = new User { Username = "Eddie" };
        _mockRepo.Setup(r => r.CreateAsync(user)).ReturnsAsync(user);

        // Act
        var result = await _userService.CreateUserAsync(user);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Id, result.Id);
        Assert.Equal(user.Username, result.Username);
    }

    [Fact]
    public async Task UpdateUserAsync_ShouldReturnTrue_WhenUpdateSucceeds()
    {
        // Arrange
        var user = new User { Id = 1, Username = "Updated Eddie" };
        _mockRepo.Setup(r => r.UpdateAsync(user)).ReturnsAsync(true);

        // Act
        var result = await _userService.UpdateUserAsync(user.Id, user);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task UpdateUserAsync_ShouldThrowArgumentException_WhenIdMismatch()
    {
        // Arrange
        var user = new User { Id = 2, Username = "Updated Eddie" };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            _userService.UpdateUserAsync(1, user)
        );
        Assert.Contains("User ID in request does not match user object.", exception.Message);
    }

    [Fact]
    public async Task UpdateUserAsync_ShouldThrowKeyNotFoundException_WhenUserDoesNotExist()
    {
        // Arrange
        var user = new User { Id = 1, Username = "Updated Eddie" };
        _mockRepo.Setup(r => r.UpdateAsync(user)).ThrowsAsync(new DbUpdateConcurrencyException());
        _mockRepo.Setup(r => r.ExistsAsync(user.Id)).ReturnsAsync(false);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _userService.UpdateUserAsync(user.Id, user)
        );
        Assert.Contains("User not found.", exception.Message);
    }

    [Fact]
    public async Task DeleteUserAsync_ShouldReturnTrue_WhenDeleteSucceeds()
    {
        // Arrange
        _mockRepo.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

        // Act
        var result = await _userService.DeleteUserAsync(1);

        // Assert
        Assert.True(result);
    }
}
