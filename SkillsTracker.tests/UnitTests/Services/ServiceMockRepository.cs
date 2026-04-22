using Moq;
using SkillsTracker.Core.Abstractions;
using SkillsTracker.Core.Models;
using SkillsTracker.Services;
using Xunit;

namespace SkillsTracker.Tests.UnitTests.Fixtures;

public class UserServiceMockRepository
{
    public Mock<IPagedRepository<User>> MockRepo { get; set; }
    public UserService UserService { get; set; }

    public UserServiceMockRepository()
    {
        MockRepo = new Mock<IPagedRepository<User>>();
        UserService = new UserService(MockRepo.Object);
    }
}
