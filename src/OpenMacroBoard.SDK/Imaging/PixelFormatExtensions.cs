using System;
using System.Drawing.Imaging;

namespace OpenMacroBoard.SDK.Imaging
{
    internal static class PixelFormatExtensions
    {
        public static byte GetBytesPerPixel(this PixelFormat format)
        {
            switch (format)
            {
                case PixelFormat.Format24bppRgb: return 3;
                case PixelFormat.Format32bppArgb: return 4;
                default:
                    throw new NotSupportedException();
            }
        }

        public static IPixelFormat ToIPixelFormat(this PixelFormat format)
        {
            switch (format)
            {
                case PixelFormat.Format32bppArgb: return PixelFormats.Bgra32;
                case PixelFormat.Format24bppRgb: return PixelFormats.Bgr24;
                default:
                    throw new NotSupportedException($"Format {format} is not supported.");
            }
        }
    }
}
