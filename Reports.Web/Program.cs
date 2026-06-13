using KassaEventsDataBase;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication
	.CreateBuilder(args);

builder.Services
	.AddRateLimiter(options => 
		options.AddFixedWindowLimiter("fixed", opt => 
		{
			opt.PermitLimit = 100;
			opt.Window = TimeSpan.FromMinutes(1);
		}))
	.AddKassaEventsDb(builder.Configuration.GetConnectionString("ClickHouse")!)
	.AddProblemDetails()
	.AddHealthChecks();

var app = builder.Build();	

app.UseRateLimiter();
app.UseExceptionHandler(); // Автоматически генерирует ProblemDetails
app.MapHealthChecks("/health");

app
	.MapGet("/report", async (KassaEventsDbContext db, CancellationToken ct) =>
	{
		var report = await db.Events
			.GroupBy(t => t.TerminalId)
			.Select(g => new
			{
				TerminalId = g.Key,
				Count = g.Count(),
				TotalAmount = g.Sum(t => t.Amount)
			})
			.ToListAsync();
		
		return Results.Ok(report);
	})
	.WithName("GetWeather")
	.RequireRateLimiting("fixed");

await app.RunAsync();
