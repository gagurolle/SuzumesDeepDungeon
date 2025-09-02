using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SuzumesDeepDungeon.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuzumesDeepDungeon.Test.IntegrationTest
{
    public class CustomWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Замените реальный DbContext на InMemory
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<DatabaseContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<DatabaseContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDatabase");
                });

                // Постройте провайдер сервисов
                var sp = services.BuildServiceProvider();

                // Создайте scope для инициализации базы данных
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<DatabaseContext>();

                    // Убедитесь, что база данных создана
                    db.Database.EnsureCreated();
                }
            });
        }
    }
}
