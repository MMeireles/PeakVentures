using PeakVentures.TechnicalTask.PixelService.Dtos;

namespace PeakVentures.TechnicalTask.PixelService.Services;

/// <summary>
/// A provider of images.
/// </summary>
internal interface IImageService
{
    /// <summary>
    /// It provides an image to be used as a Track Pixel.
    /// </summary>
    /// <returns>a <see cref="Image"/> containing both the <see cref="Image.Content"/> and the <see cref="Image.ContentType"/>.</returns>
    Image TrackPixel();
}
