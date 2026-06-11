using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KafkaFlow;
using KassEvents.Contracts;
using Microsoft.Extensions.Logging;

namespace KassaReaderService
{
    public partial class KassaEventHandler(ILogger<KassaEventHandler> logger) : IMessageHandler<KassaEvent>
    {
        public Task Handle(IMessageContext context, KassaEvent message)
        {
            Console.WriteLine("Полученно сообщенте от кассы " + message);
            LogKassaEvent(message);
            return Task.CompletedTask;
        }

        [LoggerMessage(Level = LogLevel.Information, Message = "Полученно сообщенте от кассы")]
        public partial void LogKassaEvent(KassaEvent message);
    }
}
