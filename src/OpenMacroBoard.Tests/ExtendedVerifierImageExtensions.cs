using OpenMacroBoard.Meta.TestUtils;
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
    }
}
