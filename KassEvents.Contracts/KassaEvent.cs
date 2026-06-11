namespace KassEvents.Contracts
{
    public record KassaEvent(
        string OperationType,
        string TerminalId,
        DateTime Timestamp,
        decimal Amount,
        string Currency
    );
}