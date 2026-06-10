using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace KassaEventSimulator
{
    public static class BackgroundEventProducer
    {
        public static IServiceCollection AddBackgroundEventProducer(
            this IServiceCollection services,
            HostBuilderContext hostContext
        ) =>
            services
                .AddHostedService<BackgroundEventProducerImpl>()
                .Configure<Settings>(hostContext.Configuration.GetSection("EventProducer"));

        record Settings(string BootstrapServers, string Topic);

        class BackgroundEventProducerImpl(
            IOptions<Settings> options,
            IOptions<KassaConfig> kassaOptions,
            IKassaEventsStreamGenearator eventsStreamGenearator
        ) : BackgroundService
        {
            Settings Settings { get; } = options.Value;
            KassaConfig KassaConfig { get; } = kassaOptions.Value;
            IKassaEventsStreamGenearator EventsStreamGenearator { get; } = eventsStreamGenearator;

            protected override async Task ExecuteAsync(CancellationToken cancellationToken)
            {
                var clientConfig = new ClientConfig
                {
                    BootstrapServers = Settings.BootstrapServers,
                };

                using var Producer = new ProducerBuilder<Null, KassaEvent>(clientConfig)
                    .SetValueSerializer(new JsonSerializer<KassaEvent>())
                    .Build();

                await foreach (var kassEvent in EventsStreamGenearator.GetEventsStream(cancellationToken))
                {
                    var message = new Message<Null, KassaEvent> { Value = kassEvent };
                    await Producer.ProduceAsync(Settings.Topic, message, cancellationToken);
                }
            }
        }
    }
}
