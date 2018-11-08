using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using static OpenMacroBoard.SDK.Imaging.PixelFormatConvertion;

namespace OpenMacroBoard.SDK.Imaging
{
    public unsafe static class PixelFormatConvertion
    {
        public delegate void PixelConvert(byte* sourceData, int sourceStart, byte* targetData, int targetStart);
    }

    public sealed class BitmapConverter
    {
        private readonly Dictionary<(IPixelFormat source, IPixelFormat target), PixelConvert> registeredConverters
            = new Dictionary<(IPixelFormat source, IPixelFormat target), PixelConvert>();

        public static BitmapConverter Default { get; }

        static BitmapConverter()
        {
            Default = new BitmapConverter();
            Default.RegisterDefaults();
        }

        public BitmapConverter()
        {

        }

        public void Register(IPixelFormat sourceFormat, IPixelFormat targetFormat, PixelConvert convertAction)
        {
            var key = (sourceFormat, targetFormat);
            registeredConverters.Add(key, convertAction);
        }

        public PixelConvert FindConverterFor(IPixelFormat sourceFormat, IPixelFormat targetFormat)
        {
            throw new NotImplementedException();
        }

        public KeyBitmap Convert(KeyBitmap bitmap, IPixelFormat targetFormat)
        {
            var converter = FindConverterFor(bitmap.Format, targetFormat);
            throw new NotImplementedException();
        }

        public unsafe static void BitmapTransformation(
           int width, int height,
           byte[] srcArray, int srcStride, int srcBytePerPixel,
           byte[] tarArray, int tarStride, int tarBytePerPixel,
           PixelConvert tranformationAction
        )
        {
            fixed (byte* srcData = srcArray)
            fixed (byte* tarData = srcArray)
            {
                for (int y = 0; y < height; y++)
                    for (int x = 0; x < width; x++)
                    {
                        var startSource = srcStride * y + x * srcBytePerPixel;
                        var startTarget = tarStride * y + x * tarBytePerPixel;
                        tranformationAction(srcData, startSource, tarData, startTarget);
                    }
            }
        }
    }

    internal static class BitmapConvertionsExtension
    {
        public static KeyBitmap ToKeyBitmap(this Bitmap bitmap)
        {
            var w = bitmap.Width;
            var h = bitmap.Height;

            BitmapData data = null;
            try
            {
                data = bitmap.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadOnly, bitmap.PixelFormat);

                var targetFormat = bitmap.PixelFormat;

                var targetRawBitmap = new KeyBitmap(w, h, targetStride, targetFormat);

                fixed (byte* target = targetRawBitmap.data)
                {
                    BitmapTransformation(
                        //width + height
                        w, h,

                        //source info
                        (byte*)data.Scan0, data.Stride, bitmap.PixelFormat.GetBytesPerPixel(),

                        //target info
                        target, targetStride, targetFormat.BytesPerPixel,

                        //transformation
                        FindTransformationForPair(bitmap.PixelFormat, targetFormat)
                    );
                }

                return targetRawBitmap;
            }
            finally
            {
                if (data != null)
                    bitmap.UnlockBits(data);
            }
        }
    }
}
