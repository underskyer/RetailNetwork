namespace KassaEventSimulator
{
    public record KassaConfig(
        string TerminalId,
        TimeSpan FakeEventsDelay
    );
}