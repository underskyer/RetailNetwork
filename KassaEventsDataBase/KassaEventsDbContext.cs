using Microsoft.EntityFrameworkCore;

namespace KassaEventsDataBase;

public class KassaEventsDbContext : DbContext
{
	public KassaEventsDbContext(){}
	public KassaEventsDbContext(DbContextOptions<KassaEventsDbContext> options) : base(options) {}
	public DbSet<DbKassaEvent> Events { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder
			.ApplyConfigurationsFromAssembly(typeof(KassaEventsDbContext).Assembly)
		);
	}
}