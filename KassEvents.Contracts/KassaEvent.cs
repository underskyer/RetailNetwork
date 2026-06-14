namespace KassEvents.Contracts;

public record KassaEvent(
    DateTime Timestamp,
    string TerminalId,
    string OperationType,
    string Good,
    decimal Amount,
    string Currency
);