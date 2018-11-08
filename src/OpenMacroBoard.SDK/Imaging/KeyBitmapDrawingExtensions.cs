using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace OpenMacroBoard.SDK.Imaging
{
    /// <summary>
    /// KeyBitmap factory extensions based on System.Drawing
    /// </summary>
    public static class KeyBitmapDrawingExtensions
    {
        /// <summary>
        /// Create a bitmap from encoded image stream
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="bitmapStream"></param>
        /// <returns></returns>
        public static KeyBitmap FromStream(this IKeyBitmapFactory builder, Stream bitmapStream)
        {
            using (Bitmap bitmap = (Bitmap)Image.FromStream(bitmapStream))
                return builder.FromBitmap(bitmap);
        }

        /// <summary>
        /// Create a bitmap from encoded image
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="bitmapFile"></param>
        /// <returns></returns>
        public static KeyBitmap FromFile(this IKeyBitmapFactory builder, string bitmapFile)
        {
            using (Bitmap bitmap = (Bitmap)Image.FromFile(bitmapFile))
                return builder.FromBitmap(bitmap);
        }

        /// <summary>
        /// Create key bitmap from graphics commands (for example with lambda expression)
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="graphicsAction"></param>
        /// <returns></returns>
        public static KeyBitmap FromGraphics(
            this IKeyBitmapFactory builder,
            int width,
            int height,
            Action<Graphics> graphicsAction
        )
        {
            using (var bmp = CreateKeyBitmap(width, height))
            {
                using (var g = System.Drawing.Graphics.FromImage(bmp))
                    graphicsAction(g);

                return builder.FromBitmap(bmp);
            }
        }

        /// <summary>
        /// Creates a <see cref="KeyBitmap"/> from a given <see cref="Bitmap"/>
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static KeyBitmap FromBitmap(this IKeyBitmapFactory builder, Bitmap bitmap)
            => bitmap.ToRawBgr24();

        private static Bitmap CreateKeyBitmap(int width, int height)
        {
            var img = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            return img;
        }
    }
}
