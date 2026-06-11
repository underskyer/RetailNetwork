using KafkaFlow;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace KassaReaderService
{
    public static class KafkaBusBackgroundService
    {
        public static IServiceCollection AddKafkaBusBacgroundService(this IServiceCollection services) =>
            services.AddHostedService<KafkaBusBackgroundServiceImpl>();

        class KafkaBusBackgroundServiceImpl(IKafkaBus KafkaBus) : IHostedService
        {
            public Task StartAsync(CancellationToken cancellationToken) => KafkaBus.StartAsync(cancellationToken);
            public Task StopAsync(CancellationToken _) => KafkaBus.StopAsync();
        }

    }
}