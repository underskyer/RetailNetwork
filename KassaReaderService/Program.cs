using KassaReaderService;
using Microsoft.Extensions.Hosting;

var host = Host
    .CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) => services
        .AddKassaEventsListener(hostContext)
    )
    .Build();

await host.RunAsync();