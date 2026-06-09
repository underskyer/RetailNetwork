namespace KassaEventSimulator
{
    public record KassaEvent(
        string OperationType,
        string TerminalId,
        DateTime Timestamp,
        decimal Amount,
        string Currency
    );
}