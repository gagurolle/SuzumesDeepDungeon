using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using Moq;
using SuzumesDeepDungeon.Controllers;
using SuzumesDeepDungeon.Data;
using SuzumesDeepDungeon.DTO;
using SuzumesDeepDungeon.Extensions;
using SuzumesDeepDungeon.Models;
using SuzumesDeepDungeon.Test.UnitTest;
using System.Security.Claims;
using static SuzumesDeepDungeon.Models.User;

namespace SuzumesDeepDungeon.Test.UnitTest
{
    public class DeepDungeonControllerTests
    {
        private readonly Mock<ILogger<DeepDungeon>> _loggerMock = new();
        private readonly DatabaseContext _context;
        private readonly DbContextOptions<DatabaseContext> _options;
        private readonly IConfiguration _configuration;
        public DeepDungeonControllerTests()
        {
            _options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: $"TestDatabase_{Guid.NewGuid()}").EnableSensitiveDataLogging().EnableDetailedErrors().ConfigureWarnings(warnings => warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            _context = new DatabaseContext(_options);


            //jwt
            var configValues = new Dictionary<string, string>
        {
            {"Jwt:Key", "YourSuperSecretKeyForTesting1234567890"},
            {"Jwt:Issuer", "TestIssuer"},
            {"Jwt:Audience", "TestAudience"}
        };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(configValues)
                .Build();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        private void DetachAllEntities()
        {
            foreach (var entry in _context.ChangeTracker.Entries().ToList())
            {
                entry.State = EntityState.Detached;
            }
        }

        [Fact]
        public async Task GetGameRank_ExistingId_ReturnsGameRank()
        {

            var gameRank = new MockModel().returnGamerank();

            _context.GameRanks.Add(gameRank);
            await _context.SaveChangesAsync();

            var correction = await _context.GameRanks.Where(x => x.Id == gameRank.Id).FirstAsync();

            var controller = new DeepDungeon(_loggerMock.Object, _context);

            var result = await controller.GetGameRank(1);

            result.Result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeOfType<GameRankDTO>()
            .Which.Name.Should().Be(correction.Name);


        }

        [Fact]
        public async Task GetGameRank_NonExistingId_ReturnsNotFound()
        {

            var controller = new DeepDungeon(_loggerMock.Object, _context);

            var result = await controller.GetGameRank(999);

            result.Result.Should().BeOfType<NotFoundResult>();
        }


        /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// 

        [Fact]
        public void DeleteGameRank_Method_Has_Authorize_Attribute_With_Admin_Role()
        {
            // Arrange
            var method = typeof(DeepDungeon).GetMethod("DeleteGameRank", new[] { typeof(int) });

            // Act
            var attribute = method.GetCustomAttributes(typeof(AuthorizeAttribute), true)
                .FirstOrDefault() as AuthorizeAttribute;

            // Assert
            attribute.Should().NotBeNull();
            attribute.Roles.Should().Be("Admin");
        }


        [Fact]
        public async Task DeleteGameRank_ExistingId_DeletesAndReturnsNoContent()
        {
            // Arrange
            var adminUser = await CreateTestUser("admin", "password", UserRole.Admin);
            var mockModel = new MockModel();
            var gameRank = mockModel.returnGamerank();

            gameRank.User = adminUser;
            _context.GameRanks.Add(gameRank);
            await _context.SaveChangesAsync();

            // Отсоединяем сущности, чтобы избежать конфликтов отслеживания
            DetachAllEntities();

            var controller = new DeepDungeon(_loggerMock.Object, _context);
            SetupAuthenticatedUser(controller, "admin", "Admin");

            // Act
            var result = await controller.DeleteGameRank(gameRank.Id);

            // Assert
            result.Should().BeOfType<NoContentResult>();

            // Проверяем, что запись действительно удалена
            var deletedGame = await _context.GameRanks.FindAsync(gameRank.Id);
            deletedGame.Should().BeNull();
        }

        [Fact]
        public async Task DeleteGameRank_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var adminUser = await CreateTestUser("admin", "password", UserRole.Admin);

            var controller = new DeepDungeon(_loggerMock.Object, _context);
            SetupAuthenticatedUser(controller, "admin", "Admin");

            // Act
            var result = await controller.DeleteGameRank(999);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("not found")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }


        /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// 


        [Fact]
        public async Task GetGameRanks_NoFilters_ReturnsAllGameRanks()
        {
            // Arrange
            var gameRanks = new MockModel().returnGameranks();

            _context.GameRanks.AddRange(gameRanks);
            await _context.SaveChangesAsync();

            var controller = new DeepDungeon(_loggerMock.Object, _context);

            // Act
            var result = await controller.GetGameRanks();

            _context.GameRanks.RemoveRange(gameRanks);
            await _context.SaveChangesAsync();
            // Assert
            result.Result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeAssignableTo<IEnumerable<GameRankDTO>>()
                .Which.Should().HaveCount(2);



        }

        [Fact]
        public async Task GetGameRanks_FilterByName_ReturnsFilteredResults()
        {

            // Arrange

            var gameRanks = new MockModel().returnGameranks();

            _context.GameRanks.AddRange(gameRanks);
            await _context.SaveChangesAsync();

            var controller = new DeepDungeon(_loggerMock.Object, _context);

            // Act
            var result = await controller.GetGameRanks(name: "Test");

            _context.GameRanks.RemoveRange(gameRanks);
            await _context.SaveChangesAsync();
            // Assert
            result.Result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeAssignableTo<IEnumerable<GameRankDTO>>()
                .Which.Should().ContainSingle()
                .Which.Name.Should().Be("GoodGame1Test");
        }

        [Fact]
        public async Task GetGameRanks_FilterByStatus_ReturnsFilteredResults()
        {
            // Arrange
            var gameRanks = new MockModel().returnGameranks();

            _context.GameRanks.AddRange(gameRanks);
            await _context.SaveChangesAsync();

            var controller = new DeepDungeon(_loggerMock.Object, _context);


            var correction = _context.GameRanks.Where(x => x.Status == GameStatus.Completed).ToList().Count();
            // Act
            var result = await controller.GetGameRanks(status: "Completed");

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeAssignableTo<IEnumerable<GameRankDTO>>()
                .Which.Should().ContainSingle()
                .Which.Status.Should().Be("Completed");
        }

        [Fact]
        public async Task GetGameRanks_FilterByRateRange_ReturnsFilteredResults()
        {
            // Arrange
            var gameRanks = new MockModel().returnGameranks();

            _context.GameRanks.AddRange(gameRanks);
            await _context.SaveChangesAsync();

            var controller = new DeepDungeon(_loggerMock.Object, _context);

            // Act
            var result = await controller.GetGameRanks(minRate: 3, maxRate: 4);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeAssignableTo<IEnumerable<GameRankDTO>>()
                .Which.Should().ContainSingle()
                .Which.Rate.Should().Be(4);
        }

        [Fact]
        public async Task GetGameRanks_SortByNameDesc_ReturnsSortedResults()
        {
            // Arrange

            var gameRanks = new MockModel().returnGameranks();
            _context.GameRanks.AddRange(gameRanks);
            await _context.SaveChangesAsync();

            var controller = new DeepDungeon(_loggerMock.Object, _context);

            // Act
            var result = await controller.GetGameRanks(sortBy: "name", desc: true);

            // Assert
            var sortedResults = result.Result.As<OkObjectResult>().Value.As<IEnumerable<GameRankDTO>>().ToList();
            sortedResults.Should().BeInDescendingOrder(r => r.Name);
        }

        [Fact]
        public async Task GetGameRanks_FilterByTags_ReturnsFilteredResults()
        {
            // Arrange

            var gameRanks = new MockModel().returnGameranks();

            _context.GameRanks.AddRange(gameRanks);
            await _context.SaveChangesAsync();

            var controller = new DeepDungeon(_loggerMock.Object, _context);

            // Act
            var result = await controller.GetGameRanks(tags: "JRPG");

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeAssignableTo<IEnumerable<GameRankDTO>>()
            .Which.Should().ContainSingle()
            .Which.Tags.Should().Contain(t => t.Name == "Race");
        }


        /// //////////////////////////////////////////////////////////////////////////////////////////////


        private GameRankDTO CreateTestGameRankDTO(User user)
        {

            var t = new MockModel().returnGamerank();
            t.User = user;
            var result = t.GetDTO();
            return result;

            return new GameRankDTO
            {
                Name = "Test Game",
                Rate = 5,
                Status = "Released",
                GameTime = 10.5,
                Review = "Great game",
                Image = "test.jpg",
                YoutubeLink = "https://youtube.com/test",
                MetacriticRate = 85,
                Released = DateTime.Now.AddYears(-1).ToString(),
                RawgId = "12345",
                User = new UserDTO { Username = user.Username, IsAdmin = user.IsAdmin },
                Stores = new List<StoreDTO>
            {
                new StoreDTO { RawgId = "1", StoreId = StoresEnum.Steam, Url = "https://store.test.com" }
            },
                Tags = new List<TagDTO>
            {
                new TagDTO { TagId = 1, Name = "RPG", Slug = "rpg", Language = "en", GamesCount = 100, ImageBackground = "bg.jpg" }
            },
                Achievements = new List<AchievementDTO>
            {
                new AchievementDTO { Name = "First Steps", Description = "Complete first level", ImageUrl = "achievement.jpg", CompletionPercent = "50"}
            },
                Trailers = new List<TrailerDTO>
            {
                new TrailerDTO { Name = "Launch Trailer", PreviewImageUrl = "preview.jpg", Video480p = "video480.mp4", VideoMaxQuality = "video1080.mp4" }
            },
                Screenshots = new ScreenshotDTO
                {
                    SteamHeaderUrl = "header.jpg",
                    SteamCapsuleUrl = "capsule.jpg",
                    Steam600x900Url = "600x900.jpg",
                    RawgBackgroundUrl = "rawg_bg.jpg"
                }
            };
        }

        [Fact]
        public async Task AddGameRank_AdminRole_ValidData_ReturnsCreatedResult()
        {
            // Arrange
            var user = await CreateTestUser("test", "123456", UserRole.Admin);
            var gameRankDto = CreateTestGameRankDTO(user);

            var controller = new DeepDungeon(_loggerMock.Object, _context);
            SetupAdminUser(controller);

            // Act
            var result = await controller.AddGameRank(gameRankDto);

            // Assert
            result.Result.Should().BeOfType<CreatedAtActionResult>();
            var createdResult = result.Result as CreatedAtActionResult;
            createdResult.Value.Should().BeOfType<GameRankDTO>();

            var createdGameRank = createdResult.Value as GameRankDTO;
            createdGameRank.Name.Should().Be(gameRankDto.Name);

            // Verify the game was saved to the database
            var savedGame = await _context.GameRanks.FirstOrDefaultAsync(g => g.Name == gameRankDto.Name);
            savedGame.Should().NotBeNull();
            savedGame.Name.Should().Be(gameRankDto.Name);
        }

        //[Fact]
        //public async Task AddGameRank_UserRole_ReturnsForbidden()
        //{
        //    // Arrange
        //    var user = await CreateTestUser("test", "123456", UserRole.Admin);
        //    var gameRankDto = CreateTestGameRankDTO(user);

        //    var controller = new DeepDungeon(_loggerMock.Object, _context);
        //    SetupNonAdminUser(controller);

        //    // Act
        //    var result = await controller.AddGameRank(gameRankDto);

        //    // Assert
        //    result.Result.Should().BeOfType<ForbidResult>();

        //    // Verify the game was not saved to the database
        //    var savedGame = await _context.GameRanks.FirstOrDefaultAsync(g => g.Name == gameRankDto.Name);
        //    savedGame.Should().BeNull();
        //}

        [Fact]
        public async Task AddGameRank_InvalidData_EmptyName_ReturnsBadRequest()
        {
            // Arrange
            var user = await CreateTestUser("test", "123456", UserRole.Admin);
            var gameRankDto = CreateTestGameRankDTO(user);
            gameRankDto.Name = "";

            var controller = new DeepDungeon(_loggerMock.Object, _context);
            SetupAdminUser(controller);

            // Act
            var result = await controller.AddGameRank(gameRankDto);

            // Assert
            result.Result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task AddGameRank_DuplicateName_ReturnsConflict()
        {
            // Arrange
            var user = await CreateTestUser("test", "123456", UserRole.Admin);
            var gameRankDto = CreateTestGameRankDTO(user);

            // Add the game first
            var controller = new DeepDungeon(_loggerMock.Object, _context);
            SetupAdminUser(controller);
            await controller.AddGameRank(gameRankDto);
            DetachAllEntities();

            // Try to add again
            var result = await controller.AddGameRank(gameRankDto);

            // Assert
            result.Result.Should().BeOfType<ConflictResult>();
        }

        [Fact]
        public async Task AddGameRank_NonexistentUser_ReturnsBadRequest()
        {
            // Arrange
            var user = await CreateTestUser("test", "123456", UserRole.Admin);
            var gameRankDto = CreateTestGameRankDTO(user);
            gameRankDto.User.Username = "nonexistentuser";

            var controller = new DeepDungeon(_loggerMock.Object, _context);
            SetupAdminUser(controller);

            // Act
            var result = await controller.AddGameRank(gameRankDto);

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
            var badRequestResult = result.Result as BadRequestObjectResult;
            badRequestResult.Value.Should().Be("User not found. Please provide a valid user.");
        }

        [Fact]
        public async Task AddGameRank_NoImage_UsesDefaultImage()
        {
            // Arrange
            var user = await CreateTestUser("test", "123456", UserRole.Admin);
            var gameRankDto = CreateTestGameRankDTO(user);
            gameRankDto.Image = "";

            var controller = new DeepDungeon(_loggerMock.Object, _context);
            SetupAdminUser(controller);

            // Act
            var result = await controller.AddGameRank(gameRankDto);

            // Assert
            result.Result.Should().BeOfType<CreatedAtActionResult>();
            var createdResult = result.Result as CreatedAtActionResult;
            var createdGameRank = createdResult.Value as GameRankDTO;
            createdGameRank.Image.Should().Be("default.png");

            // Verify the game was saved with default image
            var savedGame = await _context.GameRanks.FirstOrDefaultAsync(g => g.Name == gameRankDto.Name);
            savedGame.Image.Should().Be("default.png");
        }

        //[Fact]
        //public async Task UpdateGameRank_AdminRole_ValidData_ReturnsOk()
        //{
        //    // Arrange
        //    var user = await CreateTestUser("test", "123456", UserRole.Admin);
        //    var gameRankDto = CreateTestGameRankDTO(user);

        //    // First add a game
        //    var controller = new DeepDungeon(_loggerMock.Object, _context);
        //    SetupAdminUser(controller);
        //    var addResult = await controller.AddGameRank(gameRankDto);
        //    DetachAllEntities();

        //    // Get the created game ID
        //    var createdResult = addResult.Result as CreatedAtActionResult;
        //    var createdGame = createdResult.Value as GameRankDTO;
        //    var gameId = createdGame.Id;

        //    // Prepare update data
        //    var updateDto = CreateTestGameRankDTO(user);
        //    updateDto.Id = gameId;
        //    updateDto.Name = "Updated Game Name";
        //    updateDto.Rate = 4;

        //    // Act
        //    var result = await controller.UpdateGameRank(updateDto);

        //    // Assert
        //    result.Result.Should().BeOfType<OkObjectResult>();
        //    var okResult = result.Result as OkObjectResult;
        //    okResult.Value.Should().BeOfType<GameRankDTO>();

        //    var updatedGame = okResult.Value as GameRankDTO;
        //    updatedGame.Name.Should().Be("Updated Game Name");
        //    updatedGame.Rate.Should().Be(4);

        //    // Verify the game was updated in the database
        //    var savedGame = await _context.GameRanks.FirstOrDefaultAsync(g => g.Id == gameId);
        //    savedGame.Name.Should().Be("Updated Game Name");
        //    savedGame.Rate.Should().Be(4);
        //}
        [Fact]
        public async Task UpdateGameRank_AdminRole_ValidData_ReturnsOk()
        {
            // Arrange
            var user = await CreateTestUser("test", "123456", UserRole.Admin);
            var gameRankDto = CreateTestGameRankDTO(user);

            // First add a game
            var controller = new DeepDungeon(_loggerMock.Object, _context);
            SetupAdminUser(controller);
            var addResult = await controller.AddGameRank(gameRankDto);
            DetachAllEntities();

            // Get the created game ID
            var createdResult = addResult.Result as CreatedAtActionResult;
            var createdGame = createdResult.Value as GameRankDTO;
            var gameId = createdGame.Id;

            // Prepare update data
            var updateDto = CreateTestGameRankDTO(user);
            updateDto.Id = gameId;
            updateDto.Name = "Updated Game Name";
            updateDto.Rate = 4;

            // Act
            var result = await controller.UpdateGameRank(updateDto);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            okResult.Value.Should().BeOfType<GameRankDTO>();

            var updatedGame = okResult.Value as GameRankDTO;
            updatedGame.Name.Should().Be("Updated Game Name");
            updatedGame.Rate.Should().Be(4);

            // Verify the game was updated in the database
            var savedGame = await _context.GameRanks.FirstOrDefaultAsync(g => g.Id == gameId);
            savedGame.Name.Should().Be("Updated Game Name");
            savedGame.Rate.Should().Be(4);
        }

        //[Fact]
        //public async Task UpdateGameRank_UserRole_ReturnsForbidden()
        //{
        //    // Arrange
        //    var user = await CreateTestUser("test", "123456", UserRole.Admin);
        //    var gameRankDto = CreateTestGameRankDTO(user);

        //    // First add a game with admin
        //    var adminController = new DeepDungeon(_loggerMock.Object, _context);
        //    SetupAdminUser(adminController);
        //    var addResult = await adminController.AddGameRank(gameRankDto);
        //    DetachAllEntities();

        //    // Get the created game ID
        //    var createdResult = addResult.Result as CreatedAtActionResult;
        //    var createdGame = createdResult.Value as GameRankDTO;
        //    var gameId = createdGame.Id;

        //    // Try to update with user role
        //    var userController = new DeepDungeon(_loggerMock.Object, _context);
        //    SetupNonAdminUser(userController);
        //    gameRankDto.Id = gameId;
        //    gameRankDto.Name = "Updated by User";

        //    // Act
        //    var result = await userController.UpdateGameRank(gameRankDto);

        //    // Assert
        //    result.Result.Should().BeOfType<ForbidResult>();

        //    // Verify the game was not updated
        //    var savedGame = await _context.GameRanks.FirstOrDefaultAsync(g => g.Id == gameId);
        //    savedGame.Name.Should().NotBe("Updated by User");
        //}

        [Fact]
        public async Task UpdateGameRank_NonexistentGame_ReturnsNotFound()
        {
            // Arrange
            var user = await CreateTestUser("test", "123456", UserRole.Admin);
            var gameRankDto = CreateTestGameRankDTO(user);
            gameRankDto.Id = 999; // Nonexistent ID

            var controller = new DeepDungeon(_loggerMock.Object, _context);
            SetupAdminUser(controller);

            // Act
            var result = await controller.UpdateGameRank(gameRankDto);

            // Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task UpdateGameRank_NullData_ReturnsBadRequest()
        {
            // Arrange
            var controller = new DeepDungeon(_loggerMock.Object, _context);
            SetupAdminUser(controller);

            // Act
            var result = await controller.UpdateGameRank(null);

            // Assert
            result.Result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task UpdateGameRank_ChangeRawgId_RemovesAndRecreatesRelatedEntities()
        {
            // Arrange
            var user = await CreateTestUser("test", "123456", UserRole.Admin);
            var gameRankDto = CreateTestGameRankDTO(user);

            // First add a game
            var controller = new DeepDungeon(_loggerMock.Object, _context);
            SetupAdminUser(controller);
            var addResult = await controller.AddGameRank(gameRankDto);
            DetachAllEntities();

            // Get the created game ID
            var createdResult = addResult.Result as CreatedAtActionResult;
            var createdGame = createdResult.Value as GameRankDTO;
            var gameId = createdGame.Id;

            // Verify initial state
            var initialGame = await _context.GameRanks
                .Include(g => g.Stores)
                .Include(g => g.Tags)
                .Include(g => g.Achievements)
                .Include(g => g.Trailers)
                .FirstOrDefaultAsync(g => g.Id == gameId);

            var initialStoreCount = initialGame.Stores.Count;
            var initialTagCount = initialGame.Tags.Count;
            var initialAchievementCount = initialGame.Achievements.Count;
            var initialTrailerCount = initialGame.Trailers.Count;

            // Prepare update data with different RawgId
            var updateDto = CreateTestGameRankDTO(user);
            updateDto.Id = gameId;
            updateDto.RawgId = "new_rawg_id";

            // Act
            var result = await controller.UpdateGameRank(updateDto);
            DetachAllEntities();

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();

            // Verify the game was updated with new RawgId
            var updatedGame = await _context.GameRanks.FirstOrDefaultAsync(g => g.Id == gameId);
            updatedGame.RawgId.Should().Be("new_rawg_id");

            // Verify related entities were recreated
            var finalGame = await _context.GameRanks
                .Include(g => g.Stores)
                .Include(g => g.Tags)
                .Include(g => g.Achievements)
                .Include(g => g.Trailers)
                .FirstOrDefaultAsync(g => g.Id == gameId);

            // Counts should be the same as before (entities were recreated)
            finalGame.Stores.Count.Should().Be(initialStoreCount);
            finalGame.Tags.Count.Should().Be(initialTagCount);
            finalGame.Achievements.Count.Should().Be(initialAchievementCount);
            finalGame.Trailers.Count.Should().Be(initialTrailerCount);
        }



        /// //////////////////////////////////////////////////////////////////////////////////////////////


        #region SetUser
        private void SetupAdminUser(DeepDungeon controller)
        {
            var claims = new[]
            {
        new Claim(ClaimTypes.Name, "testuser"),
        new Claim(ClaimTypes.NameIdentifier, "1"),
        new Claim(ClaimTypes.Role, "Admin")
    };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            // Важно: установите IsAuthenticated в true
            identity = new ClaimsIdentity(identity.Claims, "TestAuthType", ClaimTypes.Name, ClaimTypes.Role);

            var claimsPrincipal = new ClaimsPrincipal(identity);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = claimsPrincipal
                }
            };
        }

        private void SetupNonAdminUser(DeepDungeon controller)
        {
            var claims = new[]
            {
        new Claim(ClaimTypes.Name, "testuser"),
        new Claim(ClaimTypes.NameIdentifier, "1"),
        new Claim(ClaimTypes.Role, "User")
    };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            // Важно: установите IsAuthenticated в true
            identity = new ClaimsIdentity(identity.Claims, "TestAuthType", ClaimTypes.Name, ClaimTypes.Role);

            var claimsPrincipal = new ClaimsPrincipal(identity);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = claimsPrincipal
                }
            };
        }

        private void SetupUnauthenticatedUser(DeepDungeon controller)
        {
            // Создаем пустой идентификатор без аутентификации
            var identity = new ClaimsIdentity();
            var claimsPrincipal = new ClaimsPrincipal(identity);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = claimsPrincipal
                }
            };
        }

        #endregion


        #region jwt
        // Вспомогательный метод для создания тестового пользователя
        private async Task<User> CreateTestUser(string username, string password, UserRole role)
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(12));

            var user = new User
            {
                Username = username,
                Email = $"{username}@test.com",
                Password = hashedPassword,
                Role = role
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        // Вспомогательный метод для генерации JWT токена
        private string GenerateTestToken(string username, string role)
        {
            var authController = new AuthController(_configuration, _context);

            // Используем рефлексию для вызова приватного метода
            var method = typeof(AuthController).GetMethod("GenerateJwtToken",
                         System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            return (string)method.Invoke(authController, new object[] { username, role == "Admin" });
        }

        // Вспомогательный метод для настройки аутентифицированного пользователя
        private void SetupAuthenticatedUser(DeepDungeon controller, string username, string role)
        {
            var token = GenerateTestToken(username, role);

            var claims = new[]
            {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = claimsPrincipal
                }
            };

            // Добавляем заголовок Authorization с JWT токеном
            controller.ControllerContext.HttpContext.Request.Headers["Authorization"] = $"Bearer {token}";
        }
        #endregion
    }
}