using Microsoft.EntityFrameworkCore;

namespace KassaStoreDataBase;

public class KassaStoreDbContext(DbContextOptions<KassaStoreDbContext> options) : DbContext(options)
{
	// public KassaStoreDbContext(DbContextOptions<KassaStoreDbContext> options) : base(options) {}
	public DbSet<DbGood> Goods { get; set; }
	public DbSet<DbCurrency> Currencies { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<DbGood>().HasData(
			new DbGood { Code = 1, Name = "Тапки", Count = 1001, PriceRub = 50 },
			new DbGood { Code = 2, Name = "Вертолёт", Count = 100, PriceRub = 5_000_000 },
			new DbGood { Code = 3, Name = "Шприц", Count = 1000000, PriceRub = 5 },
			new DbGood { Code = 4, Name = "Плюмбус", Count = 1001, PriceRub = 76 },
			new DbGood { Code = 5, Name = "Замок", Count = 1001, PriceRub = 42_000_000 }
		);

		modelBuilder.Entity<DbCurrency>().HasData(
			new DbCurrency { Id = 1, ShortName = "RUB", FullName = "Рубли", ToRubleRate = 1 },
			new DbCurrency { Id = 2, ShortName = "USD", FullName = "Доллары", ToRubleRate = 71.9077m },
			new DbCurrency { Id = 3, ShortName = "JPY", FullName = "Йены", ToRubleRate = 0.447911m }
		);
	}

}