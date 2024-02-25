namespace PeakVentures.TechnicalTask.Contracts;

/// <summary>
/// The Track Data contract to be used between the Producers and the Consumers.
/// </summary>
public class TrackData
{
    public const string TopicName = nameof(TrackData);
    public DateTime? Timestamp { get; init; }
    public string? Referer { get; init; }
    public string? UserAgent { get; init; }
    public string? VisitorIp { get; init; }
}
