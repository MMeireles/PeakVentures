using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using PeakVentures.TechnicalTask.Contracts;
using PeakVentures.TechnicalTask.PixelService.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton(sp => new ProducerBuilder<Null, TrackData>(new ProducerConfig
{
    BootstrapServers = sp.GetRequiredService<IConfiguration>()["Kafka:BootstrapServers"]
}).Build());
builder.Services.AddSingleton<ITrackDataProducer, TrackDataKafkaProducer>();
builder.Services.AddSingleton<IImageService>();
var app = builder.Build();
app.MapGet("/track", async (
    [FromServices] HttpContext httpContext, // It could be replace with the [FromHeader] if the VisitorIp would be sent through a custom header.
    [FromServices] ITrackDataProducer producer,
    [FromServices] IImageService imageService) =>
{
    await producer.ProduceAsync(
        httpContext.Request.Headers.Referer,
        httpContext.Request.Headers.UserAgent,
        httpContext.Connection.RemoteIpAddress?.ToString());
    var image = imageService.TrackPixel();
    return Results.Bytes(image.Content, image.ContentType);
});
app.Run();
