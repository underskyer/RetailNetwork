
using System.Runtime.CompilerServices;
using KassEvents.Contracts;

namespace KassaEventSimulator;

interface IKassaEventsStreamGenearator
{
    IAsyncEnumerable<KassaEvent> GetEventsStream(CancellationToken cancellationToken);
}