namespace PeakVentures.TechnicalTask.PixelService.Dtos;

internal class Image
{
    public byte[] Content { get; init; } = Array.Empty<byte>();
    public string ContentType { get; init; } = string.Empty;
}
