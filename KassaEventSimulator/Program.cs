using KassaEventSimulator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host
	.CreateDefaultBuilder(args)
	.ConfigureServices((hostContext, services) => services
		.AddBackgroundEventSender(hostContext)
		.AddFakeKassaEventsStreamGenearator()
		.AddOptions<KassaConfig>().BindConfiguration(KassaConfig.SectionName)
	)
	.Build();

await host.RunAsync();
