using ReportsWeb.Features;
using Scalar.AspNetCore;

namespace Reports.Web;

public static class RoutesRegistration
{
	public static WebApplication ConfigureRoutes(this WebApplication app)
	{
		app.MapHealthChecks("/health");
		app.MapOpenApi(); //if (app.Environment.IsDevelopment())
		app.MapScalarApiReference();
		app.MapKassReportEndpoints();

		return app;
	}	
}