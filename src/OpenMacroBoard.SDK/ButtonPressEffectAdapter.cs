using System.Collections.Generic;

namespace OpenMacroBoard.SDK
{
    /// <summary>
    /// Macro board adapter that implements a software button press effect.
    /// </summary>
    /// <remarks>
    /// This vaguely mimics the perspective of a real button beeing pushed
    /// and provides better feedback to the user that a button push was registered.
    /// </remarks>
    public class ButtonPressEffectAdapter : MacroBoardAdapter
    {
        private readonly Dictionary<int, KeyBitmap> mostRecentKeyBitmaps = new Dictionary<int, KeyBitmap>();
        private readonly Dictionary<int, bool> keyPressedState = new Dictionary<int, bool>();

        /// <summary>
        /// Creates a new instance of <see cref="ButtonPressEffectAdapter"/> with default configuration.
        /// </summary>
        /// <param name="macroBoard"></param>
        public ButtonPressEffectAdapter(IMacroBoard macroBoard)
            : this(macroBoard, null)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="ButtonPressEffectAdapter"/> with a given configuration.
        /// </summary>
        /// <param name="macroBoard">The board that is wrapped with the button press effect.</param>
        /// <param name="config">The configuration that should be used. If null the default configuration will be used.</param>
        public ButtonPressEffectAdapter(IMacroBoard macroBoard, ButtonPressEffectConfig config)
            : base(macroBoard)
        {
            KeyStateChanged += SoftwareButtonFeature_KeyStateChanged;
            Config = config ?? new ButtonPressEffectConfig();
        }

        /// <summary>
        /// The configuration that controls the behaviour of the button press effect feature.
        /// </summary>
        public ButtonPressEffectConfig Config { get; }

        /// <inheritdoc/>
        public override void SetKeyBitmap(int keyId, KeyBitmap bitmapData)
        {
            mostRecentKeyBitmaps[keyId] = bitmapData;
            UpdateKeyBitmap(keyId);
        }

        private void SoftwareButtonFeature_KeyStateChanged(object sender, KeyEventArgs e)
        {
            keyPressedState[e.Key] = e.IsDown;
            UpdateKeyBitmap(e.Key);
        }

        private void UpdateKeyBitmap(int keyId)
        {
            var bitmap = GetBitmapForKey(keyId);

            if (bitmap != null)
            {
                base.SetKeyBitmap(keyId, bitmap);
            }
        }

        private bool IsKeyPressed(int keyId)
        {
            if (keyPressedState.TryGetValue(keyId, out var keyPressed))
            {
                return keyPressed;
            }

            return false;
        }

        private KeyBitmap GetBitmapForKey(int keyId)
        {
            if (!mostRecentKeyBitmaps.TryGetValue(keyId, out var bitmap))
            {
                return null;
            }

            return IsKeyPressed(keyId) ? ResizeBitmap(bitmap) : bitmap;
        }

        private KeyBitmap ResizeBitmap(KeyBitmap keyBitmap)
        {
            var bitmapDataAccess = (IKeyBitmapDataAccess)keyBitmap;
            using var image = bitmapDataAccess.GetBitmap();

            return KeyBitmap.Create.FromGraphics(keyBitmap.Width, keyBitmap.Height, g =>
            {
                var scale = (float)Config.Scale;

                g.Clear(Config.BackgroundColor);

                var translateX = keyBitmap.Width * (1 - scale) * (float)Config.OriginX;
                var translateY = keyBitmap.Height * (1 - scale) * (float)Config.OriginY;

                g.TranslateTransform(translateX, translateY);
                g.ScaleTransform(scale, scale);

                if (image != null)
                {
                    g.DrawImage(image, 0f, 0f);
                }
            });
        }
    }
}
