using OpenMacroBoard.Meta.TestUtils;
using OpenMacroBoard.SDK;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Threading.Tasks;

namespace OpenMacroBoard.Tests
{
    public static class ExtendedVerifierImageExtensions
    {
        public static async Task VerifyAsync(this ExtendedVerifySettings settings, FakeMacroBoard target)
        {
            await settings
                .WithFileNameSuffix("_Meta")
                .WithExtension("txt")
                .VerifyAsync(target.GetMetaData());

            await settings
                .WithFileNameSuffix("_Image")
                .WithExtension("png")
                .VerifyAsync(target.BoardImage);
        }

        public static Task VerifyAsync(this ExtendedVerifySettings settings, KeyBitmap keyBitmap)
        {
            var dataAccess = (IKeyBitmapDataAccess)keyBitmap;

            using var img = dataAccess.ToImage();

            return settings
                .WithFileNameSuffix("_KeyBitmapImg")
                .WithExtension("png")
                .VerifyAsync(img);
        }
    }
}
