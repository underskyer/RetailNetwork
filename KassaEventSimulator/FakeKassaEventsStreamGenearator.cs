using System.Runtime.CompilerServices;
using KassaStoreDataBase;
using KassEvents.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace KassaEventSimulator;

static class FakeKassaEventsStreamGenearator
{
    public static IServiceCollection AddFakeKassaEventsStreamGenearator(this IServiceCollection services) => services
        .AddSingleton<IKassaEventsStreamGenearator, FakeKassaEventsStreamGenearatorImpl>()
    ;
    class FakeKassaEventsStreamGenearatorImpl(
        IOptions<KassaConfig> kassaOptions,
        KassaStoreDbContext db,
        ILogger<FakeKassaEventsStreamGenearatorImpl> logger
    ) : IKassaEventsStreamGenearator
    {
        KassaConfig KassaConfig { get; } = kassaOptions.Value;
        Random Random  { get; } = new Random();

        public async IAsyncEnumerable<KassaEvent> GetEventsStream([EnumeratorCancellation]CancellationToken cancellationToken)
        {
            var goods = await db.Goods.ToListAsync(cancellationToken);
            var currencies = await db.Currencies.ToListAsync(cancellationToken);
            var terminals = new List<string>{"Kass1", "Auto", "Store"};

            while (!cancellationToken.IsCancellationRequested)
            {
                var terminal = terminals[Random.Next(terminals.Count)]; // KassaConfig.TerminalId,
                var good = goods[Random.Next(goods.Count)];
                var currency = currencies[Random.Next(currencies.Count)];
                var count = Random.Next(1, 6);

                var fakeEvent = new KassaEvent(
                    Timestamp: DateTime.UtcNow,
                    OperationType: "sale",
                    TerminalId: terminal,
                    Good: good.Name,
                    Amount: count * good.PriceRub / currency.ToRubleRate,
                    Currency: currency.ShortName
                );

                good.Count -= count;
                await db.SaveChangesAsync(cancellationToken);

                logger.LogInformation("Сгенерировано фейковое событие {KassaEvent}", fakeEvent);
                
                yield return fakeEvent;

                await Task.Delay(KassaConfig.FakeEventsDelay, cancellationToken);
            }

            logger.LogInformation("Остановка рассылки: " + cancellationToken.IsCancellationRequested);
        }
    }        
}