using KassaReaderService;
using KassaEventsDataBase;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

var host = Host
    .CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) => services
        .AddKassaEventsListener(hostContext)
        .AddKassaEventsDb(hostContext.Configuration.GetConnectionString("ClickHouse")!)
    )
    .Build();

await host.RunAsync();