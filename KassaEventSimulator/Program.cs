using KassaEventSimulator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host
		.CreateDefaultBuilder(args)
		.ConfigureServices((hostContext, services) => services
				.AddBackgroundEventProducer(hostContext)
				.AddFakeKassaEventsStreamGenearator()
				.Configure<KassaConfig>(hostContext.Configuration)
		)
		.Build();

await host.RunAsync();
