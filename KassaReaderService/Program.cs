using KassaReaderService;
using KassaEventsDataBase;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

Serilog.Debugging.SelfLog.Enable(Console.Error);

var host = Host
    .CreateDefaultBuilder(args)
    .UseSerilog((context, services, configuration) => {
        configuration.ReadFrom.Configuration(context.Configuration);
    })
	.ConfigureServices((hostContext, services) => services
        .AddKassaEventsListener(hostContext)
        .AddKassaEventsDb(hostContext.Configuration.GetConnectionString("ClickHouse")!)
    )
	.Build();


await host.RunAsync();