using Microsoft.EntityFrameworkCore;
using Moq;
using SkillsTracker.Data;
using SkillsTracker.Data.Repository;
using SkillsTracker.Models;
using SkillsTracker.Services;
using SkillsTracker.Tests.UnitTests.Fixtures;

namespace SkillsTracker.Tests.UnitTests.Services;

public class UserServiceTests : IClassFixture<UserServiceMockRepository>
{
    private readonly Mock<IRepository<User>> _mockRepo;
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
        var users = names.Select(name => new User { Name = name }).ToList().AsQueryable();

        // Setup
        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(users);

        // Act
        var result = await _userService.GetUsersAsync();

        // Assert
        Assert.Equal(4, result.Count());
        foreach (var name in names)
            Assert.Contains(result, u => u.Name == name);
    }

    [Fact]
    public async Task GetUsersAsync_ShouldThrowInvalidOperationException_OnRepositoryError()
    {
        // Setup
        _mockRepo.Setup(r => r.GetAllAsync()).Throws(new Exception());

        // Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            _userService.GetUsersAsync
        );
        Assert.Contains("An error occurred while retrieving users.", exception.Message);
    }

    [Fact]
    public async Task GetUserByIdAsync_ShouldReturnCorrectUser()
    {
        // Data
        var user = new User { Id = 1, Name = "Persephone" };

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
    public async Task GetUserByIdAsync_ShouldThrowInvalidOperationException_OnRepositoryError()
    {
        // Setup
        _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).Throws(new Exception());

        // Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _userService.GetUserByIdAsync(1));
    }

    [Fact]
    public async Task CreateUserAsync_ShouldReturnCreatedUser()
    {
        // Arrange
        var user = new User { Name = "Eddie" };
        _mockRepo.Setup(r => r.CreateAsync(user)).ReturnsAsync(user);

        // Act
        var result = await _userService.CreateUserAsync(user);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Id, result.Id);
        Assert.Equal(user.Name, result.Name);
    }

    [Fact]
    public async Task CreateUserAsync_ShouldThrowInvalidOperationException_OnRepositoryError()
    {
        // Arrange
        var user = new User { Name = "Eddie" };
        _mockRepo.Setup(r => r.CreateAsync(user)).ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _userService.CreateUserAsync(user)
        );
        Assert.Contains("An error occurred while creating the user.", exception.Message);
    }

    [Fact]
    public async Task UpdateUserAsync_ShouldReturnTrue_WhenUpdateSucceeds()
    {
        // Arrange
        var user = new User { Id = 1, Name = "Updated Eddie" };
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
        var user = new User { Id = 2, Name = "Updated Eddie" };

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
        var user = new User { Id = 1, Name = "Updated Eddie" };
        _mockRepo.Setup(r => r.UpdateAsync(user)).ThrowsAsync(new DbUpdateConcurrencyException());
        _mockRepo.Setup(r => r.ExistsAsync(user.Id)).ReturnsAsync(false);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _userService.UpdateUserAsync(user.Id, user)
        );
        Assert.Contains("User not found.", exception.Message);
    }

    [Fact]
    public async Task UpdateUserAsync_ShouldThrowInvalidOperationException_OnGeneralError()
    {
        // Arrange
        var user = new User { Id = 1, Name = "Updated Eddie" };
        _mockRepo.Setup(r => r.UpdateAsync(user)).ThrowsAsync(new Exception("Unexpected error"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _userService.UpdateUserAsync(user.Id, user)
        );
        Assert.Contains("An error occurred while updating the user.", exception.Message);
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

    [Fact]
    public async Task DeleteUserAsync_ShouldThrowInvalidOperationException_OnDbUpdateException()
    {
        // Arrange
        _mockRepo.Setup(r => r.DeleteAsync(1)).ThrowsAsync(new DbUpdateException("Delete failed"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _userService.DeleteUserAsync(1)
        );
        Assert.Contains("Error deleting user.", exception.Message);
    }
}
