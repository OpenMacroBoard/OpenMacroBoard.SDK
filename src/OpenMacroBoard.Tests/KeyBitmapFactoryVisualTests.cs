using OpenMacroBoard.Meta.TestUtils;
using OpenMacroBoard.SDK;
using System.Drawing;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using VerifyXunit;
using Xunit;

namespace OpenMacroBoard.Tests
{
    [UsesVerify]
    public class KeyBitmapFactoryVisualTests
    {
        public ExtendedVerifySettings Verifier { get; } = DefaultVerifySettings.Build();

        [Fact]
        [SupportedOSPlatform("windows")]
        public async Task GDI_arc_is_drawn_correctly()
        {
            Verifier.Initialize();
            Verifier.UseFileNameAsDirectory();

            var key = KeyBitmap.Create
                .FromGraphics(
                    width: 64,
                    height: 64,
                    g =>
                    {
                        g.Clear(Color.Red);
                        g.DrawArc(Pens.Blue, new Rectangle(10, 10, 40, 40), 10, 180);
                    }
                );

            await Verifier.VerifyAsync(key);
        }
    }
}
