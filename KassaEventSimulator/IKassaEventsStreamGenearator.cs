
using System.Runtime.CompilerServices;

namespace KassaEventSimulator
{
    interface IKassaEventsStreamGenearator
    {
        IAsyncEnumerable<KassaEvent> GetEventsStream(CancellationToken cancellationToken);
    }
}