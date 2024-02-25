namespace PeakVentures.TechnicalTask.PixelService.Services;

/// <summary>
/// A producer of the track data.
/// </summary>
internal interface ITrackDataProducer
{
    /// <summary>
    /// It provides the track data to consumers.
    /// </summary>
    /// <param name="referer">The referer data being tracked.</param>
    /// <param name="userAgent">The user agent of th</param>
    /// <param name="visitorIp"></param>
    /// <param name="cancellationToken">a token to cancel the operation.</param>
    /// <returns>the producing task.</returns>
    Task ProduceAsync(string? referer, string? userAgent, string? visitorIp, CancellationToken cancellationToken = default);
}
