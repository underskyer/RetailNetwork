
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace KassaEventSimulator
{
    static class FakeKassaEventsStreamGenearator
    {
        public static IServiceCollection AddFakeKassaEventsStreamGenearator(this IServiceCollection services) =>
            services
                .AddSingleton<IKassaEventsStreamGenearator, FakeKassaEventsStreamGenearatorImpl>()
            ;

        record FakeKassaEventsStreamGenearatorImpl(IOptions<KassaConfig> kassaOptions) : IKassaEventsStreamGenearator
        {
            KassaConfig KassaConfig { get; } = kassaOptions.Value;
            Random Random  { get; } = new Random();

            public async IAsyncEnumerable<KassaEvent> GetEventsStream([EnumeratorCancellation]CancellationToken cancellationToken)
            {
                while (!cancellationToken.IsCancellationRequested)
                {                
                    yield return new KassaEvent(
                        OperationType: "sale",
                        TerminalId: KassaConfig.TerminalId,
                        Timestamp: DateTime.UtcNow,
                        Amount: Random.Next(50, 200),
                        Currency: "USD"
                    );
                    await Task.Delay(KassaConfig.FakeEventsDelay, cancellationToken);
                }

            }

        }
        
    }
}