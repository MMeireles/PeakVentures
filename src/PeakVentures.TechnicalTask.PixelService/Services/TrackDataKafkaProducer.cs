using Confluent.Kafka;
using PeakVentures.TechnicalTask.Contracts;

namespace PeakVentures.TechnicalTask.PixelService.Services;

/// <summary>
/// A <see cref="ITrackDataProducer"/> designed for Kafka.
/// </summary>
internal class TrackDataKafkaProducer : ITrackDataProducer
{
    public TrackDataKafkaProducer(IProducer<Null, TrackData> producer) =>
        Producer = producer;

    private readonly IProducer<Null, TrackData> Producer;

    public Task ProduceAsync(string? referer, string? userAgent, string? visitorIp, CancellationToken cancellationToken) =>
        Producer.ProduceAsync(TrackData.TopicName, new Message<Null, TrackData>
        {
            Value = new TrackData
            {
                Referer = referer,
                Timestamp = DateTime.UtcNow,
                UserAgent = userAgent,
                VisitorIp = visitorIp,
            }
        }, cancellationToken);
}
