using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace KassaEventsDataBase
{
	/// <summary>
	/// для миграций
	/// </summary>
    public class KassaEventsDbContextFactory : IDesignTimeDbContextFactory<KassaEventsDbContext>
    {
        public KassaEventsDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<KassaEventsDbContext>();
            
            // Используйте синтаксически валидную строку
            // ClickHouse провайдер не будет реально подключаться при генерации скрипта
            optionsBuilder.UseClickHouse("Server=fake_server;Database=fake_db;Trusted_Connection=True;");
            
            return new KassaEventsDbContext(optionsBuilder.Options);
        }
    }
}