using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KassaEventsDataBase;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Caching.Hybrid;
using Serilog;

namespace Reports.Web;
public static class ServicesRegistration
{
	public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
	{
		builder.Services.AddServices(builder.Configuration);
		builder.Host.UseSerilog((context, services, configuration) =>
			configuration.ReadFrom.Configuration(context.Configuration)
		);
		return builder;
	}

    public static IServiceCollection AddServices(
		this IServiceCollection services,
		ConfigurationManager config
	) => services
		.AddRateLimiter(options => 
			options.AddFixedWindowLimiter("fixed", opt => 
			{
				opt.PermitLimit = 100;
				opt.Window = TimeSpan.FromMinutes(1);
			})
		)
		.AddStackExchangeRedisCache(options =>
		{
			options.Configuration = config.GetConnectionString("Redis");
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
		.AddKassaEventsDb(config.GetConnectionString("ClickHouse")!)
		.AddProblemDetails()
		.AddOpenApi()
		.AddHealthChecks()
		.Services;

	
}