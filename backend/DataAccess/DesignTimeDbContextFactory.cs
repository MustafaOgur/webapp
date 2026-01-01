using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Core.Utilities.Security;
using System;
using System.IO;

namespace DataAccess
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../WebAPI"))
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            // Dummy CurrentUserService: Migration için gerekli, gerçek değer kullanılmaz
            var dummyUserService = new DummyCurrentUserService();
            return new AppDbContext(optionsBuilder.Options, dummyUserService);
        }
    }

    // Migration sırasında kullanacağımız fake service
    public class DummyCurrentUserService : ICurrentUserService
    {
        public string UserId => Guid.Empty.ToString(); // boş veya sabit id yeterli

        public string Role => "User";
    }
}
