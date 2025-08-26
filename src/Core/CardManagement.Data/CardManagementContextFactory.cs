using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace CardManagement.Data.Db
{
    public class CardManagementContextFactory : IDesignTimeDbContextFactory<CardManagementContext>
    {
        public CardManagementContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "Backoffice", "Backoffice.API"))
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<CardManagementContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

            return new CardManagementContext(optionsBuilder.Options);
        }
    }
}