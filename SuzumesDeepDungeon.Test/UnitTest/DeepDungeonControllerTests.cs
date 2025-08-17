using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using SuzumesDeepDungeon.Controllers;
using SuzumesDeepDungeon.Data;
using SuzumesDeepDungeon.DTO;
using SuzumesDeepDungeon.Models;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SuzumesDeepDungeon.Tests.UnitTests;

public class DeepDungeonControllerTests
{
    private readonly Mock<ILogger<DeepDungeon>> _loggerMock = new();
    private readonly Mock<DatabaseContext> _contextMock = new();
    private readonly Mock<DbSet<GameRank>> _gameRanksMock = new();
    private readonly Mock<DbSet<User>> _usersMock = new();

    public DeepDungeonControllerTests()
    {
        _contextMock.Setup(x => x.GameRanks).Returns(_gameRanksMock.Object);
        _contextMock.Setup(x => x.Users).Returns(_usersMock.Object);
    }

    [Fact]
    public async Task DeleteGameRank_ExistingId_ReturnsNoContent()
    {
        // Arrange
        var gameRank = new GameRank { Id = 1 };
        _gameRanksMock.Setup(x => x.FindAsync(1))
            .ReturnsAsync(gameRank);

        var controller = new DeepDungeon(_loggerMock.Object, _contextMock.Object);

        // Act
        var result = await controller.DeleteGameRank(1);

        // Assert
        result.Should().BeOfType<NoContentResult>();
        _gameRanksMock.Verify(x => x.Remove(gameRank), Times.Once);
        _contextMock.Verify(x => x.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task DeleteGameRank_NonExistingId_ReturnsNotFound()
    {
        // Arrange
        _gameRanksMock.Setup(x => x.FindAsync(1))
            .ReturnsAsync(null as GameRank);

        var controller = new DeepDungeon(_loggerMock.Object, _contextMock.Object);

        // Act
        var result = await controller.DeleteGameRank(1);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
        _contextMock.Verify(x => x.SaveChangesAsync(default), Times.Never);
    }

    [Fact]
    public async Task AddGameRank_ValidData_ReturnsCreatedResult()
    {
        // Arrange
        var newGameRankDto = new GameRankDTO
        {
            Name = "New Game",
            User = new UserDTO { Username = "testUser" }
        };

        var user = new User { Username = "testUser" };

        _usersMock.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(), default))
            .ReturnsAsync(user);

        _gameRanksMock.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<GameRank, bool>>>(), default))
            .ReturnsAsync(null as GameRank);

        var controller = new DeepDungeon(_loggerMock.Object, _contextMock.Object);

        // Act
        var result = await controller.AddGameRank(newGameRankDto);

        // Assert
        var createdAtActionResult = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
        createdAtActionResult.ActionName.Should().Be(nameof(DeepDungeon.GetGameRank));
        _gameRanksMock.Verify(x => x.Add(It.IsAny<GameRank>()), Times.Once);
        _contextMock.Verify(x => x.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task AddGameRank_DuplicateName_ReturnsConflict()
    {
        // Arrange
        var newGameRankDto = new GameRankDTO
        {
            Name = "Existing Game",
            User = new UserDTO { Username = "testUser" }
        };

        _gameRanksMock.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<GameRank, bool>>>(), default))
            .ReturnsAsync(new GameRank());

        var controller = new DeepDungeon(_loggerMock.Object, _contextMock.Object);

        // Act
        var result = await controller.AddGameRank(newGameRankDto);

        // Assert
        result.Result.Should().BeOfType<ConflictObjectResult>();
        _contextMock.Verify(x => x.SaveChangesAsync(default), Times.Never);
    }
}