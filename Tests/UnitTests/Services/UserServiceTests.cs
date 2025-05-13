using Microsoft.EntityFrameworkCore;
using Moq;
using SkillsTracker.Data;
using SkillsTracker.Models;
using SkillsTracker.Services;
using Xunit;

namespace SkillsTracker.Tests.UnitTests.Services;

public class UserServiceTests
{
    private readonly UserService _userService;
    private readonly Mock<ApplicationDbContext> _mockContext;

    public UserServiceTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

        _mockContext = new Mock<ApplicationDbContext>(options);
        _userService = new UserService(_mockContext.Object);
    }

    [Fact]
    public async Task GetUsersAsync_ShouldReturnUsers()
    {
        // Arrange
        string[] names = ["Brandon", "Alexa", "Sally", "Jon"];
        var users = names.Select(name => new User { Name = name }).ToList();

        var mockSet = new Mock<DbSet<User>>();
        _mockContext.Setup(c => c.Users).Returns(mockSet.Object);
        mockSet.Setup(m => m.ToListAsync(It.IsAny<CancellationToken>())).ReturnsAsync(users);

        // Act
        var result = await _userService.GetUsersAsync();

        // Assert
        Assert.Equal(4, result.Count());
        foreach (var name in names)
            Assert.Contains(result, u => u.Name == name);
    }

    [Fact]
    public async Task GetUserByIdAsync_ShouldReturnCorrectUser()
    {
        // Arrange
        var testUser = new User { Id = 1, Name = "Eddie" };
        var mockSet = new Mock<DbSet<User>>();

        _mockContext.Setup(c => c.Users).Returns(mockSet.Object);
        mockSet.Setup(m => m.FindAsync(1)).ReturnsAsync(testUser);

        // Act
        var result = await _userService.GetUserByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Eddie", result.Name);
    }

    [Fact]
    public async Task UpdateUserAsync_ShouldReturnFalse_WhenIdMismatch()
    {
        // Arrange
        var user = new User { Id = 2, Name = "Updated Eddie" };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _userService.UpdateUserAsync(1, user));
    }
}
