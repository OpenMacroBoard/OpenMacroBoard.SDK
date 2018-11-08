using OpenMacroBoard.SDK;
using System;

namespace OpenMacroBoard.SDK.Imaging
{
    /// <summary>
    /// A basic factory extension to create <see cref="KeyBitmap"/>s
    /// </summary>
    public static class KeyBitmapBasicExtensions
    {
        /// <summary>
        /// Creates a single color (single pixel) <see cref="KeyBitmap"/> with a given color.
        /// </summary>
        /// <param name="builder">The builder that is used to create the <see cref="KeyBitmap"/></param>
        /// <param name="r">Red channel.</param>
        /// <param name="g">Green channel.</param>
        /// <param name="b">Blue channel.</param>
        /// <returns></returns>
        public static KeyBitmap FromRgb(this IKeyBitmapFactory builder, byte r, byte g, byte b)
        {
            //If everything is 0 (black) take a shortcut ;-)
            if (r == 0 && g == 0 && b == 0)
                return KeyBitmap.Black;

            var key = new KeyBitmap(1, 1, 3, PixelFormats.Bgr24);
            key.data[0] = b;
            key.data[1] = g;
            key.data[2] = r;

            return key;
        }

        public static KeyBitmap FromArray(this IKeyBitmapFactory builder, int width, int height, int stride, IPixelFormat format, byte[] data)
        {
            var minStride = width * format.BytesPerPixel;
            if (stride < minStride)
                throw new ArgumentException($"{nameof(stride)} must be >= than {nameof(width)}x{nameof(format)}.{nameof(IPixelFormat.BytesPerPixel)}");

            var expectedDataLength = stride * height;
            if (data != null && data.Length != expectedDataLength)
                throw new ArgumentException($"{nameof(data)}.Length must be equal to {nameof(stride)}x{nameof(height)}");

            var key = new KeyBitmap(width, height, stride, format);
            key.data = (byte[])data?.Clone();

            return key;
        }

        public static KeyBitmap FromNoOverhangBgr24Array(this IKeyBitmapFactory builder, int width, int height, byte[] data)
            => builder.FromArray(width, height, width * 3, PixelFormats.Bgr24, data);
    }
}
