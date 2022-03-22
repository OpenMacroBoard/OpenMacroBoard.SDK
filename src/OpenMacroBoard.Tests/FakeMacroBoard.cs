using OpenMacroBoard.SDK;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Text;

namespace OpenMacroBoard.Tests
{
    public sealed class FakeMacroBoard : IMacroBoard
    {
        private readonly GridKeyLayout keyPosition;
        private readonly Image<Bgr24> logoImage;

        private byte brightness = 100;
        private bool isLogoShown = false;
        private bool disposed = false;
        private bool isConnected = true;

        public FakeMacroBoard(GridKeyLayout keyPosition)
        {
            this.keyPosition = keyPosition ?? throw new ArgumentNullException(nameof(keyPosition));

            BoardImage = new Image<Bgra32>(
                keyPosition.Area.Width + 2 * keyPosition.GapSize,
                keyPosition.Area.Height + 2 * keyPosition.GapSize,
                Color.FromRgba(0, 0, 0, 200)
            );

            logoImage = CreateLogoImage();

            ShowLogo();
        }

        public IKeyLayout Keys => keyPosition;

        public bool IsConnected
        {
            get => isConnected;
            set
            {
                if (value == isConnected)
                {
                    return;
                }

                // Order when isConnected is set is important
                // because method calls might not work when the state
                // is set to "disconnected"

                if (!isConnected)
                {
                    isConnected = true;
                    ShowLogo();
                }
                else
                {
                    this.ClearKeys();
                    isConnected = false;
                }

                ConnectionStateChanged?.Invoke(this, new ConnectionEventArgs(isConnected));
            }
        }

        public Image<Bgra32> BoardImage { get; }

        public event EventHandler<KeyEventArgs> KeyStateChanged;
        public event EventHandler<ConnectionEventArgs> ConnectionStateChanged;

        public void Dispose()
        {
            VerifyNotDisposed();

            disposed = true;

            BoardImage.Dispose();
            logoImage.Dispose();
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="percent"/> is grater than 100%.</exception>
        public void SetBrightness(byte percent)
        {
            VerifyNotDisposed();

            if (!isConnected)
            {
                return;
            }

            if (percent > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(percent));
            }

            brightness = percent;
        }

        public void SetKeyBitmap(int keyId, KeyBitmap bitmapData)
        {
            VerifyNotDisposed();

            if (!isConnected)
            {
                return;
            }

            if (isLogoShown)
            {
                // set isLogoShown to false before clearing everything,
                // because clearing internally calls SetKeyBitmap, which would
                // cause a StackOverflow exception if isLogoShown is still true
                isLogoShown = false;
                this.ClearKeys();
            }

            var dataAccess = (IKeyBitmapDataAccess)bitmapData;

            if (dataAccess.IsEmpty)
            {
                using var blackImage = new Image<Bgr24>(keyPosition.KeySize, keyPosition.KeySize);
                ResizeAndApplyImage(keyId, blackImage);
            }
            else
            {
                // load given image
                using var keyImage = Image.LoadPixelData<Bgr24>(
                    dataAccess.GetData(),
                    bitmapData.Width,
                    bitmapData.Height
                );

                ResizeAndApplyImage(keyId, keyImage);
            }
        }

        public void TriggerKeyEvent(int keyId, bool down)
        {
            if (!isConnected)
            {
                return;
            }

            KeyStateChanged?.Invoke(this, new KeyEventArgs(keyId, down));
        }

        public void ShowLogo()
        {
            VerifyNotDisposed();

            if (!isConnected)
            {
                return;
            }

            this.DrawFullScreenBitmap(logoImage);

            // set isLogoShown after drawing the image, 
            // because drawing the image calls SetKeyBitmap
            // which internally sets isLogoShown to false
            isLogoShown = true;
        }

        public string GetMetaData()
        {
            var builder = new StringBuilder();

            builder.Append("IsConnected:   ").Append(isConnected).AppendLine();

            if (isConnected)
            {
                builder
                    .Append("Brightness:    ").Append(brightness).AppendLine()
                    .Append("IsLogoShown:   ").Append(isLogoShown).AppendLine()
                    ;
            }

            return builder.ToString();
        }

        public string GetFirmwareVersion()
        {
            return "fake-firmware";
        }

        public string GetSerialNumber()
        {
            return "fake-serial-number";
        }

        private Image<Bgr24> CreateLogoImage()
        {
            using var logoImg = AssetLoader.LoadImage("OpenMacroBoard-Logo.png");

            const double scalar = 0.95;

            var maxHeight = keyPosition.Area.Height * scalar;
            var maxWidth = keyPosition.Area.Width * scalar;

            var scaleFactorY = maxHeight / logoImg.Height;
            var scaleFactorX = maxWidth / logoImg.Width;

            var scaleFactor = Math.Min(scaleFactorX, scaleFactorY);

            var newHeight = (int)Math.Round(logoImg.Height * scaleFactor);
            var newWidth = (int)Math.Round(logoImg.Width * scaleFactor);

            var resizeOptions = new ResizeOptions()
            {
                Size = new Size(newWidth, newHeight),
                Sampler = KnownResamplers.NearestNeighbor,
            };

            logoImg.Mutate(x => x.Resize(resizeOptions));

            var totalImage = new Image<Bgr24>(keyPosition.Area.Width, keyPosition.Area.Height);

            var leftOffset = (keyPosition.Area.Width - newWidth) / 2;
            var topOffset = (keyPosition.Area.Height - newHeight) / 2;

            totalImage.Mutate(x => x.DrawImage(logoImg, new Point(leftOffset, topOffset), 1));

            return totalImage;
        }

        private void VerifyNotDisposed()
        {
            if (disposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }
        }

        private void ResizeAndApplyImage(int keyId, Image<Bgr24> image)
        {
            if (image.Width != keyPosition.KeySize || image.Height != keyPosition.KeySize)
            {
                // resize to target key size
                image.Mutate(x => x.Resize(keyPosition.KeySize, keyPosition.KeySize));
            }

            // draw to target position
            var targetPosition = keyPosition[keyId];

            BoardImage.Mutate(x => x.DrawImage(
                image,
                new Point(
                    targetPosition.X + keyPosition.GapSize,
                    targetPosition.Y + keyPosition.GapSize
                ),
                1
            ));
        }
    }
}
