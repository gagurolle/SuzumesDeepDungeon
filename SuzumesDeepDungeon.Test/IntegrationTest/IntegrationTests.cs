// IntegrationTests/DeepDungeonControllerIntegrationTests.cs
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SuzumesDeepDungeon.Data;
using SuzumesDeepDungeon.DTO;
using SuzumesDeepDungeon.Models;
using System.Net;
using System.Net.Http.Json;

namespace SuzumesDeepDungeon.Tests.IntegrationTests;

public class DeepDungeonControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly DatabaseContext _dbContext;

    public DeepDungeonControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
        _dbContext = factory.Services.CreateScope().ServiceProvider
            .GetRequiredService<DatabaseContext>();
        SeedDatabase();
    }

    private void SeedDatabase()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Database.EnsureCreated();

        var user = new User { Username = "testUser" };
        var games = new List<GameRank>
        {
            new() { Name = "Game A", Rate = 85, Created = DateTime.Now.AddDays(-1), User = user },
            new() { Name = "Game B", Rate = 92, Created = DateTime.Now, User = user },
            new() { Name = "Game C", Rate = 75, Created = DateTime.Now.AddDays(-2), User = user }
        };

        _dbContext.Users.Add(user);
        _dbContext.GameRanks.AddRange(games);
        _dbContext.SaveChanges();
    }

    [Fact]
    public async Task GetGameRank_FilterByName_ReturnsCorrectResults()
    {
        // Act
        var response = await _client.GetAsync("/DeepDungeon?name=Game B");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<IEnumerable<GameRankDTO>>();
        result.Should().HaveCount(1);
        result.First().Name.Should().Be("Game B");
    }

    [Fact]
    public async Task GetGameRank_SortByRateDesc_ReturnsOrderedResults()
    {
        // Act
        var response = await _client.GetAsync("/DeepDungeon?sortBy=rate&desc=true");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = (await response.Content.ReadFromJsonAsync<IEnumerable<GameRankDTO>>()).ToList();
        result.Should().HaveCount(3);
        result.Select(g => g.Rate).Should().BeInDescendingOrder();
    }

    [Fact]
    public async Task GetGameRank_FilterByMinRate_ReturnsFilteredResults()
    {
        // Act
        var response = await _client.GetAsync("/DeepDungeon?minRate=80");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<IEnumerable<GameRankDTO>>();
        result.Should().HaveCount(2);
        result.All(g => g.Rate >= 80).Should().BeTrue();
    }
    [Fact]
    public async Task AddGameRank_ValidRequest_CreatesInDatabase()
    {
        // Arrange
        var newGame = new GameRankDTO
        {
            Name = "New Integration Game",
            User = new UserDTO { Username = "testUser" },
            Rate = 88
        };

        // Act
        var response = await _client.PostAsJsonAsync("/DeepDungeon", newGame);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var createdGame = await _dbContext.GameRanks
            .FirstOrDefaultAsync(g => g.Name == "New Integration Game");

        createdGame.Should().NotBeNull();
        createdGame.Rate.Should().Be(88);
        createdGame.User.Username.Should().Be("testUser");
    }
}