using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace FreshInventory.Infrastructure.Data.Context
{
    public class FreshInventoryDbContextFactory : IDesignTimeDbContextFactory<FreshInventoryDbContext>
    {
        public FreshInventoryDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<FreshInventoryDbContext>();
            optionsBuilder.UseSqlite(connectionString);

            return new FreshInventoryDbContext(optionsBuilder.Options);
        }
    }
}
