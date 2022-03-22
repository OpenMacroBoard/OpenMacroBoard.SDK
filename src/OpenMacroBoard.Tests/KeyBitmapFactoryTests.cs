using FluentAssertions;
using OpenMacroBoard.SDK;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Xunit;

namespace StreamDeckSharp.Tests
{
    public class KeyBitmapFactoryTests
    {
        [Fact]
        public void RgbFactoryShouldCreateASinglePixelKeyBitmap()
        {
            byte red = 100;
            byte green = 200;
            byte blue = 0;

            var expectation = KeyBitmap.FromBgr24Array(1, 1, new byte[] { blue, green, red });
            var key = KeyBitmap.Create.FromRgb(red, green, blue);

            key.Should().Be(expectation);
        }

        [Fact]
        public void RgbFactoryShouldCreateANullDataElementForBlack()
        {
            var expectation = KeyBitmap.FromBgr24Array(1, 1, null);
            var wrongResult = KeyBitmap.FromBgr24Array(1, 1, new byte[] { 0, 0, 0 });

            var key = KeyBitmap.Create.FromRgb(0, 0, 0);
            key.Should().Be(expectation);
            key.Should().NotBe(wrongResult);
        }

        [Fact]
        public void PixelFormatIsRgbLeftToRightAndTopToBottom()
        {
            var expectation = KeyBitmap.FromBgr24Array(2, 2, new byte[2 * 2 * 3]
            {
                000, 001, 002,  010, 011, 012,
                020, 021, 022,  030, 031, 032,
            });

            var topLeft = new Bgr24(2, 1, 0);
            var topRight = new Bgr24(12, 11, 10);
            var bottomLeft = new Bgr24(22, 21, 20);
            var bottomRight = new Bgr24(32, 31, 30);

            var img = new Image<Bgr24>(2, 2);

            img[0, 0] = topLeft;
            img[1, 0] = topRight;
            img[0, 1] = bottomLeft;
            img[1, 1] = bottomRight;

            var key = KeyBitmap.Create.FromImageSharpImage(img);
            key.Should().Be(expectation);
        }
    }
}
