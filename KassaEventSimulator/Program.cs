using KassaEventSimulator;
using KassaStoreDataBase;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

var host = Host
	.CreateDefaultBuilder(args)
    .UseSerilog((context, services, configuration) =>
        configuration.ReadFrom.Configuration(context.Configuration)
    )
	.ConfigureServices((hostContext, services) => services
		.AddBackgroundEventSender(hostContext)
		.AddKassaStoreDb(hostContext.Configuration.GetConnectionString("Postgres")!)
		.AddFakeKassaEventsStreamGenearator()
		.AddOptions<KassaConfig>().BindConfiguration(KassaConfig.SectionName)
	)
	.Build();

await host.RunAsync();
