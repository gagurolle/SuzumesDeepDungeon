using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using SuzumesDeepDungeon;
using SuzumesDeepDungeon.Data;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Xunit;

public class AuthIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public AuthIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    //[Fact]
    //public async Task DeleteGameRank_WithoutAuth_ReturnsUnauthorized()
    //{
    //    // Arrange
    //    var client = _factory.CreateClient();

    //    // Act
    //    var response = await client.DeleteAsync("/api/deepdungeon/1");

    //    // Assert
    //    response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    //}

    //[Fact]
    //public async Task DeleteGameRank_WithUserRole_ReturnsForbidden()
    //{
    //    // Arrange
    //    var client = _factory.CreateClient();

    //    // Получите токен для пользователя с ролью User
    //    var token = await GetUserToken("testuser", "password", false);
    //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

    //    // Act
    //    var response = await client.DeleteAsync("/api/deepdungeon/1");

    //    // Assert
    //    response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    //}

    //[Fact]
    //public async Task DeleteGameRank_WithAdminRole_ReturnsNoContent()
    //{
    //    // Arrange
    //    var client = _factory.CreateClient();

    //    // Получите токен для пользователя с ролью Admin
    //    var token = await GetAdminToken("admin", "password");
    //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

    //    // Сначала создайте игру для удаления
    //    var gameResponse = await client.PostAsync("/api/deepdungeon",
    //        new StringContent(JsonSerializer.Serialize(new { Name = "Test Game" }),
    //        Encoding.UTF8, "application/json"));

    //    var gameId = await gameResponse.Content.ReadAsStringAsync();

    //    // Act
    //    var response = await client.DeleteAsync($"/api/deepdungeon/{gameId}");

    //    // Assert
    //    response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    //}

    //private async Task<string> GetUserToken(string username, string password, bool isAdmin = false)
    //{
    //    var client = _factory.CreateClient();

    //    var loginData = new
    //    {
    //        Username = username,
    //        Password = password
    //    };

    //    var response = await client.PostAsync("/api/auth/login",
    //        new StringContent(JsonSerializer.Serialize(loginData),
    //        Encoding.UTF8, "application/json"));

    //    var content = await response.Content.ReadAsStringAsync();
    //    var tokenResponse = JsonSerializer.Deserialize<JsonElement>(content);

    //    return tokenResponse.GetProperty("token").GetString();
    //}

    //private async Task<string> GetAdminToken(string username, string password)
    //{
    //    // Сначала создайте администратора
    //    var client = _factory.CreateClient();

    //    var registrationData = new
    //    {
    //        Username = username,
    //        Password = password,
    //        IsAdmin = true
    //    };

    //    await client.PostAsync("/api/auth/registration",
    //        new StringContent(JsonSerializer.Serialize(registrationData),
    //        Encoding.UTF8, "application/json"));

    //    // Затем получите токен
    //    return await GetUserToken(username, password, true);
    //}
}
