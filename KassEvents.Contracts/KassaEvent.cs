using ProtoBuf;

namespace KassEvents.Contracts;

[ProtoContract(SkipConstructor = true)]
public record KassaEvent(
    [property: ProtoMember(1)] DateTime Timestamp,
    [property: ProtoMember(2)] string TerminalId,
    [property: ProtoMember(3)] OperationType OperationType,
    [property: ProtoMember(4)] string Good,
    [property: ProtoMember(5)] decimal Amount,
    [property: ProtoMember(6)] string Currency
);