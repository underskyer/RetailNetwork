using KassaEventsDataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.EntityFrameworkCore;

namespace ReportsWeb.Features;

public static class KassReportEndpoint
{
	public static IEndpointRouteBuilder MapKassReportEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes
			.MapGroup("/reports")
            .WithTags("Отчёты")
			.WithName("KassReport")
			.RequireRateLimiting("fixed");

		group.MapGet("/kassReport/{terminalId}", Handle);

        return routes;
    }

	public record GetKassReportRequest([FromRoute] string terminalId);
	public record GetKassReportResponse(
		string good,
		int count,
		decimal TotalAmount
	);

	public static async Task<List<GetKassReportResponse>> Handle(
        [AsParameters] GetKassReportRequest request, // Автоматическая привязка
        KassaEventsDbContext db,
		HybridCache cache,
		CancellationToken ct
	) => await cache.GetOrCreateAsync(
		key: $"report:{request.terminalId}:goods",
		async _ => await GetReportFromDb(request.terminalId, db, ct),
		tags: ["kass reports"],
		cancellationToken: ct
	);

	static async Task<List<GetKassReportResponse>> GetReportFromDb(
        string terminalId, // Автоматическая привязка
        KassaEventsDbContext db,
		CancellationToken ct
	) => await db.Events
		.Where(ev => ev.TerminalId == terminalId)
		.GroupBy(t => t.Good)
		.Select(g => new GetKassReportResponse(
			good: g.Key,
			g.Count(),
			g.Sum(t => t.Amount)
		))
		.ToListAsync(ct);
}
