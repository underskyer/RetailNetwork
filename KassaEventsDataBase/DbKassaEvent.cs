using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ClickHouse.EntityFrameworkCore.Extensions;

namespace KassaEventsDataBase
{

	[EntityTypeConfiguration(typeof(DbKassaEventConfiguration))]
	public class DbKassaEvent
	{
        public Guid Id { get; set; }
		public DateTime Timestamp { get; set; }
        public string TerminalId { get; set; } = default!;
        public decimal Amount { get; set; }
		public Dictionary<string, string>? Metadata { get; set; }
	}

	public class DbKassaEventConfiguration : IEntityTypeConfiguration<DbKassaEvent>
	{
		public void Configure(EntityTypeBuilder<DbKassaEvent> builder)
		{
			builder
			.ToTable("kass_events", table => table
				.HasMergeTreeEngine()
				.WithPrimaryKey("id", "timestamp")
			);

			builder
				.Property(e => e.Metadata)
				.HasColumnType("Map(String, String)");

			builder.HasKey(e => e.Id);
		}
	}
}