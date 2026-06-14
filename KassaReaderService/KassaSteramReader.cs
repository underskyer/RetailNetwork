using KafkaFlow;
using KafkaFlow.Serializer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Hosting;
using Confluent.Kafka;

namespace KassaReaderService;

public static partial class KassaSteramReader
{
    public static IServiceCollection AddKassaEventsListener(
        this IServiceCollection services,
        HostBuilderContext hostContext
    )
    {
        var cfgSection = hostContext.Configuration.GetSection(Settings.SectionName);
        var settings = cfgSection.Get<Settings>()!;

        return services
            .AddKafkaFlowHostedService(kafka => kafka
                .UseConsoleLog()
                .AddCluster(cluster => cluster
                    .WithBrokers(settings.BootstrapServers.Split(",", StringSplitOptions.RemoveEmptyEntries))
                    .CreateTopicIfNotExists(settings.Topic, settings.NumberOfPartitions, settings.ReplicationFactor)
                    .AddConsumer(consumer => consumer
                        .Topic(settings.Topic)
                        .WithGroupId(settings.ConsumersGroupId)
                        .WithBufferSize(settings.MessageBufferSize)
                        .WithWorkersCount(settings.WorkersCount)
                        .AddMiddlewares(m => m
                            .AddDeserializer<JsonCoreDeserializer>()
                            .AddTypedHandlers(h => h
                                .AddHandler<KassaEventHandler>()
                            )
                        )
                    )
                )
            )
            .AddOptions<Settings>().BindConfiguration(Settings.SectionName).Services;
    }

    record Settings()
    {
        public string BootstrapServers { get; init; } = default!;
        public string Topic { get; init; } = default!;
        public int NumberOfPartitions { get; init; } = default!;
        public short ReplicationFactor { get; init; } = default!;
        public string ConsumersGroupId { get; init; } = default!;
        public int MessageBufferSize { get; init; } = default!;
        public int WorkersCount { get; init; } = default!;
        public static string SectionName => "EventConsumer";
    }

}