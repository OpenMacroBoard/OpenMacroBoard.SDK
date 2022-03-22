using OpenMacroBoard.Meta.TestUtils;
using OpenMacroBoard.SDK;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Threading.Tasks;
using VerifyXunit;
using Xunit;

namespace OpenMacroBoard.Tests
{
    [UsesVerify]
    public class KeyBitmapVisualTests
    {
        public ExtendedVerifySettings Verifier { get; } = DefaultVerifySettings.Build();

        [Theory]
        [ClassData(typeof(DeviceGridKeyPositionDataProvider))]
        public async Task FakeMacroboardSelfCheck(string deviceName, GridKeyLayout keys)
        {
            Verifier.Initialize();

            Verifier
                .UseFileNameAsDirectory()
                .UseFileName(deviceName)
                ;

            using var board = new FakeMacroBoard(keys);
            int stateCnt = 0;

            Task VerifyBoardStateAsync(string stateName)
            {
                var currentCount = stateCnt;
                stateCnt++;

                return Verifier
                    .WithFileNameSuffix($"_{currentCount:D3}")
                    .WithFileNameSuffix($"_{stateName}")
                    .VerifyAsync(board);
            }

            await VerifyBoardStateAsync("AfterConstruction");

            var red = KeyBitmap.Create.FromColor(OmbColor.Red);
            board.SetKeyBitmap(red);

            await VerifyBoardStateAsync("EverythingRed");

            board.ClearKeys();

            await VerifyBoardStateAsync("BoardCleared");

            using var fullImg = AssetLoader.LoadImage("ExampleImageText.jpg");
            board.DrawFullScreenBitmap(fullImg);

            await VerifyBoardStateAsync("FullscreenDrawn");

            board.ShowLogo();

            await VerifyBoardStateAsync("ShowLogo");

            var blue = KeyBitmap.Create.FromColor(OmbColor.AliceBlue);
            board.SetKeyBitmap(2, blue);

            await VerifyBoardStateAsync("SingleKeyAfterLogo");

            board.IsConnected = false;

            await VerifyBoardStateAsync("DisconnectState");

            // send key in disconnected state should do nothing.
            board.SetKeyBitmap(3, blue);

            await VerifyBoardStateAsync("DisconnectStateSetBitmap");

            board.IsConnected = true;

            await VerifyBoardStateAsync("ReconnectedState");
        }

        [Fact]
        public async Task ButtonPressEffectWorkingAsExpected()
        {
            Verifier.Initialize();
            Verifier.UseFileNameAsDirectory();

            var underlyingBoard = new FakeMacroBoard(DeviceGridKeyPositionDataProvider.Medium);
            using var board = underlyingBoard.WithButtonPressEffect();

            var keyImage = new Image<Bgr24>(72, 72);

            keyImage.Mutate(x =>
            {
                var circle = new EllipsePolygon(36, 36, 20);
                x.Fill(Color.Green, circle);
            });

            var key = KeyBitmap.Create.FromImageSharpImage(keyImage);

            board.SetKeyBitmap(7, key);

            await Verifier.WithFileName("0.BitmapSet").VerifyAsync(underlyingBoard);

            underlyingBoard.TriggerKeyEvent(7, true);

            await Verifier.WithFileName("1.KeyPressed").VerifyAsync(underlyingBoard);

            underlyingBoard.TriggerKeyEvent(7, false);

            await Verifier.WithFileName("2.KeyReleased").VerifyAsync(underlyingBoard);
        }

        [Fact]
        public async Task DisconnectReplayAdapterWorksAsExpected()
        {
            Verifier.Initialize();
            Verifier.UseFileNameAsDirectory();

            var underlyingBoard = new FakeMacroBoard(DeviceGridKeyPositionDataProvider.Medium);
            using var board = underlyingBoard.WithDisconnectReplay();

            var key = KeyBitmap.Create.FromColor(OmbColor.Magenta);

            board.SetBrightness(70);
            board.SetKeyBitmap(7, key);

            await Verifier.WithFileName("0.StateApplied").VerifyAsync(underlyingBoard);

            underlyingBoard.IsConnected = false;

            await Verifier.WithFileName("1.Disconnected").VerifyAsync(underlyingBoard);

            underlyingBoard.IsConnected = true;

            await Verifier.WithFileName("2.Reconnected").VerifyAsync(underlyingBoard);
        }
    }
}
