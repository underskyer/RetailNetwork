using KafkaFlow;
using KafkaFlow.Serializer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Hosting;
using Confluent.Kafka;
using KafkaFlow.Configuration;

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
					.CreateTopicIfNotExists(settings.KassEventsTopic, settings.NumberOfPartitions, settings.ReplicationFactor)
					.AddConsumer(ConfigureConsumer)
				)
			)
			.AddOptions<Settings>().BindConfiguration(Settings.SectionName).Services;

		void ConfigureConsumer(IConsumerConfigurationBuilder consumer) => consumer
			.Topic(settings.KassEventsTopic)
			.WithGroupId(settings.ConsumersGroupId)
			.WithBufferSize(settings.MessageBufferSize)
			.WithWorkersCount(settings.WorkersCount)
			.AddMiddlewares(m => m
				.AddDeserializer<ProtobufNetDeserializer>()
				.AddTypedHandlers(h => h
					.AddHandler<KassaEventHandler>()
				)
			);
	}

	record Settings()
    {
        public string BootstrapServers { get; init; } = default!;
        public string KassEventsTopic { get; init; } = default!;
        public int NumberOfPartitions { get; init; } = default!;
        public short ReplicationFactor { get; init; } = default!;
        public string ConsumersGroupId { get; init; } = default!;
        public int MessageBufferSize { get; init; } = default!;
        public int WorkersCount { get; init; } = default!;
        public static string SectionName => "EventConsumer";
    }

}