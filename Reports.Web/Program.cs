using KassaEventsDataBase;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Hybrid;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication
	.CreateBuilder(args);

builder.Services
	.AddRateLimiter(options => 
		options.AddFixedWindowLimiter("fixed", opt => 
		{
			opt.PermitLimit = 100;
			opt.Window = TimeSpan.FromMinutes(1);
		}))
	.AddStackExchangeRedisCache(options =>
	{
		options.Configuration = builder.Configuration.GetConnectionString("Redis");
		options.InstanceName = "KassReports_"; // Префикс для всех ключей
	})
	.AddHybridCache(options =>
	{
		options.MaximumPayloadBytes = 1024 * 1024;  // Максимальный размер 1MB
		options.DefaultEntryOptions = new HybridCacheEntryOptions
		{
			Expiration = TimeSpan.FromMinutes(30),           // Абсолютное истечение (L2)
			LocalCacheExpiration = TimeSpan.FromMinutes(5)   // Локальное истечение (L1)
		};
	}).Services
	.AddKassaEventsDb(builder.Configuration.GetConnectionString("ClickHouse")!)
	.AddProblemDetails()
	.AddOpenApi()
	.AddHealthChecks();

builder.Host.UseSerilog((context, services, configuration) =>
	configuration.ReadFrom.Configuration(context.Configuration)
);

var app = builder.Build();	

app.UseSerilogRequestLogging();
app.UseRateLimiter();
app.UseExceptionHandler(); // Автоматически генерирует ProblemDetails
app.MapHealthChecks("/health");
app.MapOpenApi(); //if (app.Environment.IsDevelopment())
app.MapScalarApiReference();

app
	.MapGet("/kassReport/{terminalId}", async (
		string terminalId,
		KassaEventsDbContext db,
		HybridCache cache,
		CancellationToken ct) =>
	{
		var cacheKey = $"report:{terminalId}:goods";

		var report = await cache.GetOrCreateAsync(
			cacheKey,
			async _ => await db.Events
				.Where(ev => ev.TerminalId == terminalId)
				.GroupBy(t => t.Good)
				.Select(g => new
				{
					Good = g.Key,
					Count = g.Count(),
					TotalAmount = g.Sum(t => t.Amount)
				})
				.ToListAsync(),
			tags: ["kass reports"],
			cancellationToken: ct
		);
		
		return Results.Ok(report);
	})
	.WithName("GetWeather")
	.RequireRateLimiting("fixed");

await app.RunAsync();
