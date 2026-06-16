using Serilog;

namespace Reports.Web;
public static class MiddlewareRegistration
{
	public static WebApplication ConfigureMiddlewarePipeline(this WebApplication app)
	{
		app.UseSerilogRequestLogging();
		app.UseRateLimiter();
		app.UseExceptionHandler(); // Автоматически генерирует ProblemDetails
		return app;
	}	
}