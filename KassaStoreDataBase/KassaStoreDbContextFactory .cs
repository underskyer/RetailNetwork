using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace KassaStoreDataBase;

/// <summary>
/// для миграций
/// </summary>
public class KassaStoreDbContextFactory : IDesignTimeDbContextFactory<KassaStoreDbContext>
{
    public KassaStoreDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<KassaStoreDbContext>()
            .UseNpgsql("Server=fake_server;Database=fake_db;Trusted_Connection=True;")
            .Options;
        
        return new KassaStoreDbContext(options);
    }
}