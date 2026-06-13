using KafkaFlow;
using KassEvents.Contracts;
using Microsoft.Extensions.Logging;
using KassaEventsDataBase;

namespace KassaReaderService
{
    public partial class KassaEventHandler(
		ILogger<KassaEventHandler> logger,
		KassaEventsDbContext db
	) : IMessageHandler<KassaEvent>
    {
        public async Task Handle(IMessageContext context, KassaEvent message)
        {
            LogKassaEvent(message);

			var dbMessage = new DbKassaEvent
			{
				Timestamp = message.Timestamp,
				TerminalId = message.TerminalId,
				Amount = message.Amount,
				Metadata = new () {
					["Currency"] = message.Currency.ToString(),
					["OperationType"] = message.OperationType,
				}
			};

			await db.Events.AddAsync(dbMessage); // AddRangeAsync
            await db.SaveChangesAsync();

            Console.WriteLine("!!!!!   Сообщение сохранено в БД");
        }

        [LoggerMessage(Level = LogLevel.Information, Message = "Полученно сообщенте от кассы: {message}")]
        public partial void LogKassaEvent(KassaEvent message);
    }
}
