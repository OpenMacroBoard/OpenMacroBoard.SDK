using System;

namespace OpenMacroBoard.SDK.Imaging
{
    internal unsafe static class DefaultBitmapConvertionsExtensions
    {
        public static void RegisterDefaults(this BitmapConverter converter)
        {
            converter.Register(PixelFormats.Bgra32, PixelFormats.Bgr24, Transform_Brga32ToBgr24);
        }

        private static void Transform_Brga32ToBgr24(byte* sourceData, int sourceStart, byte* targetData, int targetStart)
        {
            double alpha = (double)sourceData[sourceStart + 3] / 255f;
            targetData[targetStart + 0] = (byte)Math.Round(sourceData[sourceStart + 0] * alpha);
            targetData[targetStart + 1] = (byte)Math.Round(sourceData[sourceStart + 1] * alpha);
            targetData[targetStart + 2] = (byte)Math.Round(sourceData[sourceStart + 2] * alpha);
        }
    }
}
