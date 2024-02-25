using Confluent.Kafka;
using PeakVenture.TechnicalTask.StorageService;
using PeakVentures.TechnicalTask.Contracts;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton(sp => new ConsumerBuilder<Ignore, TrackData>(new ConsumerConfig
{
    BootstrapServers = sp.GetRequiredService<IConfiguration>()["Kafka:BootstrapServers"]
}).Build());
builder.Services.AddHostedService<TrackDataKafkaConsumer>();
var app = builder.Build();
app.Run();
