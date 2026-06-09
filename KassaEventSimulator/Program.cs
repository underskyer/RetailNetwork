using KassaEventSimulator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

await Host
    .CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) => services
        .AddBackgroundEventProducer(hostContext)
        .AddFakeKassaEventsStreamGenearator()
        .Configure<KassaConfig>(hostContext.Configuration)
    )                
    .Build()
    .RunAsync();
