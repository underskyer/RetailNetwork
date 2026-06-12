namespace KassEvents.Contracts
{
    public record KassaEvent(
        DateTime Timestamp,
        string TerminalId,
        string OperationType,
        decimal Amount,
        string Currency
    );
}