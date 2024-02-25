using Confluent.Kafka;
using Moq;
using PeakVentures.TechnicalTask.Contracts;
using PeakVentures.TechnicalTask.PixelService.Services;

namespace PeakVentures.TechnicalTask.PixelService.Tests.Services;

public class TrackDataKafkaProducerTests
{
    [Theory]
    [InlineData("https://google.com", "SomeUserAgent 1.2.3", "192.168.1.1")]
    [InlineData("https://google.com", "SomeUserAgent 1.2.3", null)]
    [InlineData("https://google.com", null, null)]
    [InlineData(null, "SomeUserAgent 1.2.3", null)]
    [InlineData(null, null, null)]
    public async Task WhenProduceAsyncIsInvoked_ThenProducerIsInvoked(string? referer, string? userAgent, string? visitorIp)
    {
        // Arrange
        var cancellationToken = new CancellationToken();
        var producer = MockProducer();
        var target = new TrackDataKafkaProducer(producer.Object);

        // Act
        await target.ProduceAsync(referer, userAgent, visitorIp, cancellationToken);

        // Assert
        producer.Verify(mock => mock
            .ProduceAsync(
                It.Is<string>(input =>
                    TrackData.TopicName.Equals(input)),
                It.Is<Message<Null, TrackData>>(input =>
                    (input.Value.Referer == null ? referer == null : input.Value.Referer.Equals(referer)) &&
                    (input.Value.UserAgent == null ? userAgent == null : input.Value.UserAgent.Equals(userAgent)) &&
                    (input.Value.VisitorIp == null ? visitorIp == null : input.Value.VisitorIp.Equals(visitorIp))),
                It.Is<CancellationToken>(input =>
                    cancellationToken.Equals(input))),
            Times.Once);
    }

    [Fact]
    public async Task WhenProducerThrowsException_ThenExceptionIsPropagated()
    {
        // Arrange
        var referer = "https://bing.com";
        var userAgent = "AnotherUserAgent 4.5.6";
        var visitorIp = "10.0.0.1";
        var cancellationToken = new CancellationToken();
        var expectedException = new Exception("Producer Exception");
        var producer = MockProducer(exception: expectedException);
        var target = new TrackDataKafkaProducer(producer.Object);

        // Assert
        var result = await Assert.ThrowsAsync<Exception>(() =>
            // Act
            target.ProduceAsync(referer, userAgent, visitorIp, cancellationToken));

        producer.Verify(mock => mock
            .ProduceAsync(
                It.Is<string>(input =>
                    TrackData.TopicName.Equals(input)),
                It.Is<Message<Null, TrackData>>(input =>
                    (input.Value.Referer == null ? referer == null : input.Value.Referer.Equals(referer)) &&
                    (input.Value.UserAgent == null ? userAgent == null : input.Value.UserAgent.Equals(userAgent)) &&
                    (input.Value.VisitorIp == null ? visitorIp == null : input.Value.VisitorIp.Equals(visitorIp))),
                It.Is<CancellationToken>(input =>
                    cancellationToken.Equals(input))),
            Times.Once);
        Assert.Equal(expectedException, result);
    }

    #region Setup
    private static Mock<IProducer<Null, TrackData>> MockProducer(DeliveryResult<Null, TrackData>? result = default, Exception? exception = default)
    {
        var mock = new Mock<IProducer<Null, TrackData>>();
        if (exception is not null)
        {
            mock
                .Setup(producer => producer.ProduceAsync(It.IsAny<string>(), It.IsAny<Message<Null, TrackData>>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(exception)
                .Verifiable();
        }
        if (result is not null)
        {
            mock
                .Setup(producer => producer.ProduceAsync(It.IsAny<string>(), It.IsAny<Message<Null, TrackData>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(result)
                .Verifiable();
        }
        return mock;
    }
    #endregion
}
