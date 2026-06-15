using ProtoBuf;

namespace KassEvents.Contracts;

[ProtoContract]
public enum OperationType
{
    /// <summary>
    /// Значение по умолчанию для Kafka
    /// </summary>
    [ProtoMember(0)] Unknown = 0,

    /// <summary>
    /// Продажа
    /// </summary>
    [ProtoMember(1)] Sale = 1,
    
    /// <summary>
    /// Возврат
    /// </summary>
    [ProtoMember(2)] Refund = 2,
}