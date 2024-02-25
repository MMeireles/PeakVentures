using PeakVentures.TechnicalTask.PixelService.Dtos;
using System.Net.Mime;
using System.Text;

namespace PeakVentures.TechnicalTask.PixelService.Services;

internal class HardCodedImageService : IImageService
{
    public Image TrackPixel() => new()
    {
        Content = Encoding.UTF8.GetBytes("R0lGODlhAQABAAAAACH5BAEAAAAALAAAAAABAAEAAAIBAAA="), // More info: https://stackoverflow.com/a/15960901
        ContentType = MediaTypeNames.Image.Gif,
    };
}
