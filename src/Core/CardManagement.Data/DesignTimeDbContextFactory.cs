using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CardManagement.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<CardManagementContext>
{
    public CardManagementContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<CardManagementContext>();
        
        // Use SQL Server Authentication for migrations
        var connectionString = "Server=localhost,1433;Database=CardManagementDB;User Id=SA;Password=reallyStrongPwd123;TrustServerCertificate=true;"
        
        optionsBuilder.UseSqlServer(connectionString);
        
        return new CardManagementContext(optionsBuilder.Options);
    }
} 