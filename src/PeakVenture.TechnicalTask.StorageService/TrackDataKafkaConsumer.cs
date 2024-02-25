using Confluent.Kafka;
using PeakVentures.TechnicalTask.Contracts;

namespace PeakVenture.TechnicalTask.StorageService;

internal class TrackDataKafkaConsumer : BackgroundService
{
    public TrackDataKafkaConsumer(IConfiguration configuration, IConsumer<Ignore, TrackData> consumer)
    {
        Consumer = consumer;
        FileAbsoluteName = configuration["Log:Visitors"]
            ?? "/tmp/visits.log"; // TODO: add log to warn that file name is not being provided
    }

    private readonly IConsumer<Ignore, TrackData> Consumer;
    private readonly string FileAbsoluteName;

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        Consumer.Subscribe(TrackData.TopicName);
        try
        {
            using var file = File.Exists(FileAbsoluteName)
                ? File.AppendText(FileAbsoluteName)
                : File.CreateText(FileAbsoluteName); // FIXME: if the business requirement is to use append-only than handle differently when the file does not exist
            while (!cancellationToken.IsCancellationRequested)
            {
                await ConsumeMessage(file, cancellationToken);
            }
        }
        finally
        {
            Consumer.Close();
        }
    }

    private async Task ConsumeMessage(StreamWriter file, CancellationToken cancellationToken)
    {
        try
        {
            var trackData = Consumer.Consume(cancellationToken).Message.Value;
            if (!string.IsNullOrEmpty(trackData.VisitorIp))
            {
                var timestamp = (trackData.Timestamp ?? DateTime.UtcNow).ToString("o");
                var referer = string.IsNullOrEmpty(trackData.Referer) ? "null" : trackData.Referer;
                var userAgent = string.IsNullOrEmpty(trackData.UserAgent) ? "null" : trackData.UserAgent;
                await file.WriteAsync($"{timestamp}|{referer}|{userAgent}|{trackData.VisitorIp}{Environment.NewLine}");
            }
            // TODO: handle cases in which the mandatory Visitor IP address is missing.
        }
        catch
        {
            throw; // FIXME: keep the throw in order not to silence the exception without it being properly handled.
            // TODO: handle exception properly, like logging and using a fallback strategy
        }
    }
}
