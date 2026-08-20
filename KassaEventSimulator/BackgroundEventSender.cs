using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using KafkaFlow.Serializer;
using KafkaFlow;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Confluent.Kafka;
using KassEvents.Contracts;

namespace KassaEventSimulator;

public static class BackgroundEventSender
{
    public static IServiceCollection AddBackgroundEventSender(
        this IServiceCollection services,
        HostBuilderContext hostContext
    )
    {
        using var loggerFcatory = new LoggerFactory();
        var cfgSection = hostContext.Configuration.GetSection(Settings.SectionName);
        var settings = cfgSection.Get<Settings>()!;

        var logger = loggerFcatory.CreateLogger("BackgroundEventSender");
        logger.LogInformation("Запуск с конфигурацией " + settings);
        

        return services
            .AddKafkaFlowHostedService(k => k
                .UseConsoleLog()
                .AddCluster(c => c
                    .WithBrokers(settings.BootstrapServers.Split(",", StringSplitOptions.RemoveEmptyEntries))
                    .CreateTopicIfNotExists(settings.KassEventsTopic, 1, 1)
                    .AddProducer<KassaEvent>(p => p
                        .WithProducerConfig(cfgSection.Get<ProducerConfig>()!)
                        .AddMiddlewares(m => m
                            .AddSerializer<ProtobufNetSerializer>()
                        )
                    )
                )
            )
            .AddHostedService<BackgroundEventSenderImpl>()
            .AddOptions<Settings>()
                .BindConfiguration(Settings.SectionName)
                .ValidateOnStart()
                .Services;
    }

    record Settings()
    {
        public string BootstrapServers { get; init; } = default!;
        public string ProducerName { get; init; } = default!;
        public string KassEventsTopic { get; init; } = default!;

        public static string SectionName => "EventProducer";
    }

    class BackgroundEventSenderImpl(
        IOptions<Settings> options,
        ILogger<BackgroundEventSenderImpl> logger,
        IMessageProducer<KassaEvent> MessageProducer,            
        IKassaEventsStreamGenearator EventsStreamGenearator
    ) : BackgroundService
    {
        Settings Settings { get; } = options.Value;

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            static string makeKey(KassaEvent ev) => $"Terminal:{ev.TerminalId}";

            await foreach (var kassEvent in EventsStreamGenearator.GetEventsStream(cancellationToken))
            {
                await MessageProducer.ProduceAsync(
                    Settings.KassEventsTopic,
                    makeKey(kassEvent),
                    kassEvent
                );

                logger.LogInformation("Отрправлено кассовое событие {Event}", kassEvent);
            }

            logger.LogInformation("рассылка завершена: " + cancellationToken.IsCancellationRequested);

        }
    }
}
