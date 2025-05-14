using Moq;
using SkillsTracker.Data.Repository;
using SkillsTracker.Models;
using SkillsTracker.Services;
using Xunit;

namespace SkillsTracker.Tests.UnitTests.Fixtures;

public class UserServiceMockRepository
{
    public Mock<IRepository<User>> MockRepo { get; set; }
    public UserService UserService { get; set; }

    public UserServiceMockRepository()
    {
        MockRepo = new Mock<IRepository<User>>();
        UserService = new UserService(MockRepo.Object);
    }
}
