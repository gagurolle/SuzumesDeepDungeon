// IntegrationTests/WebAppFactory.cs
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using SuzumesDeepDungeon.Data;

namespace SuzumesDeepDungeon.Tests.IntegrationTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Заменяем реальную БД на InMemory
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<DatabaseContext>));

            if (descriptor != null) services.Remove(descriptor);

            services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseInMemoryDatabase("TestDb");
            });
        });
    }
}