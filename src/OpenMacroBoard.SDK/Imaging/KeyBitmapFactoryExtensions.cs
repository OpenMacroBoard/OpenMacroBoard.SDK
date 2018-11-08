using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace OpenMacroBoard.SDK.Imaging
{
    /// <summary>
    /// KeyBitmap factory extensions based on System.Windows (WPF)
    /// </summary>
    public static class KeyBitmapFactoryExtensions
    {
        /// <summary>
        /// Uses a WPF FrameworkElement to create a keyImage
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public static KeyBitmap FromWpfElement(
            this IKeyBitmapFactory builder,
            int width,
            int height,
            FrameworkElement e
        )
        {
            //Do WPF layout process manually (because the element is not a UI element)
            e.Measure(new Size(width, height));
            e.Arrange(new Rect(0, 0, width, height));
            e.UpdateLayout();

            //Render the element as bitmap
            RenderTargetBitmap renderer = new RenderTargetBitmap(width, height, 96, 96, System.Windows.Media.PixelFormats.Pbgra32);
            renderer.Render(e);

            //Convert to StreamDeck compatible format
            var pbgra32 = new byte[width * height * 4];
            renderer.CopyPixels(pbgra32, width * 4, 0);

            var bgrTarget = new byte[width * height * 3];

            //var bitmapData = ConvertPbgra32ToBgr24(pbgra32, width, height);
            BitmapConvertions.BitmapTransformation(
                width, height,
                pbgra32, width * 4, 4,
                bgrTarget, width * 3, 3,
                BitmapConvertions.Bgra32ToBgr24
            );

            return new KeyBitmap(width, height, width * 3, PixelFormats.Bgr24);
        }
    }
}
