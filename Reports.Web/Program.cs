var app = WebApplication
	.CreateBuilder(args)
	.ConfigureServices()
	.Build();	

await app
	.ConfigureMiddlewarePipeline()
	.ConfigureRoutes()
	.RunAsync();
