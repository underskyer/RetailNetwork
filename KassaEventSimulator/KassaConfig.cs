namespace KassaEventSimulator;

public record KassaConfig()
{
    public string TerminalId { get; init; } = default!;
    public TimeSpan FakeEventsDelay { get; init; }
    public static string SectionName => "Kassa";
}