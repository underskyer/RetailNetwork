using System.Runtime.CompilerServices;
using KassEvents.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace KassaEventSimulator
{
    static class FakeKassaEventsStreamGenearator
    {
        public static IServiceCollection AddFakeKassaEventsStreamGenearator(this IServiceCollection services) => services
            .AddSingleton<IKassaEventsStreamGenearator, FakeKassaEventsStreamGenearatorImpl>()
        ;
        class FakeKassaEventsStreamGenearatorImpl(
            IOptions<KassaConfig> kassaOptions,
            ILogger<FakeKassaEventsStreamGenearatorImpl> logger
        ) : IKassaEventsStreamGenearator
        {
            KassaConfig KassaConfig { get; } = kassaOptions.Value;
            Random Random  { get; } = new Random();

            public async IAsyncEnumerable<KassaEvent> GetEventsStream([EnumeratorCancellation]CancellationToken cancellationToken)
            {
                var goods = new List<string>{"Тапки", "Вертолёт", "Шприц"};
                var currencies = new List<string>{"USD", "RUB", "JPY"};
                var terminals = new List<string>{"Kass1", "Auto", "Store"};

                while (!cancellationToken.IsCancellationRequested)
                {                
                    var fakeEvent = new KassaEvent(
                        OperationType: "sale",
                        TerminalId: terminals[Random.Next(terminals.Count)], // KassaConfig.TerminalId,
                        Good: goods[Random.Next(goods.Count)],
                        Timestamp: DateTime.UtcNow,
                        Amount: Random.Next(50, 200),
                        Currency: currencies[Random.Next(currencies.Count)]
                    );
                    logger.LogInformation("Сгенерировано фейковое событие {KassaEvent}", fakeEvent);
                    
                    yield return fakeEvent;

                    await Task.Delay(KassaConfig.FakeEventsDelay, cancellationToken);
                }

                logger.LogInformation("Остановка рассылки: " + cancellationToken.IsCancellationRequested);
            }
        }        
    }
}