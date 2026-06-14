using KassaEventSimulator;
using KassaStoreDataBase;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host
	.CreateDefaultBuilder(args)
	.ConfigureServices((hostContext, services) => services
		.AddBackgroundEventSender(hostContext)
		.AddKassaStoreDb(hostContext.Configuration.GetConnectionString("Postgres")!)
		.AddFakeKassaEventsStreamGenearator()
		.AddOptions<KassaConfig>().BindConfiguration(KassaConfig.SectionName)
	)
	.Build();

await host.RunAsync();
