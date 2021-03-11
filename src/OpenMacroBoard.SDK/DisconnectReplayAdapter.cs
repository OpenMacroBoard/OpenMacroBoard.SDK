using System.Collections.Generic;

namespace OpenMacroBoard.SDK
{
    /// <summary>
    /// A <see cref="IMacroBoard"/> adapter that replays brightness and keybitmaps if a device is disconnected.
    /// </summary>
    public class DisconnectReplayAdapter : MacroBoardAdapter
    {
        private readonly Dictionary<int, KeyBitmap> mostRecentKeyBitmaps = new Dictionary<int, KeyBitmap>();
        private byte? mostRecentBrightness = null;

        /// <summary>
        /// Creates a new instance of <see cref="DisconnectReplayAdapter"/>.
        /// </summary>
        /// <param name="macroBoard"></param>
        public DisconnectReplayAdapter(IMacroBoard macroBoard)
            : base(macroBoard)
        {
            ConnectionStateChanged += DisconnectReplayAdapter_ConnectionStateChanged;
        }

        private void DisconnectReplayAdapter_ConnectionStateChanged(object sender, ConnectionEventArgs e)
        {
            if (e.NewConnectionState)
            {
                // devices connected again: replay last known values

                if (mostRecentBrightness.HasValue)
                {
                    base.SetBrightness(mostRecentBrightness.Value);
                }

                foreach (var bmp in mostRecentKeyBitmaps)
                {
                    base.SetKeyBitmap(bmp.Key, bmp.Value);
                }
            }
        }

        /// <inheritdoc/>
        public override void SetBrightness(byte percent)
        {
            mostRecentBrightness = percent;
            base.SetBrightness(percent);
        }

        /// <inheritdoc/>
        public override void SetKeyBitmap(int keyId, KeyBitmap bitmapData)
        {
            mostRecentKeyBitmaps[keyId] = bitmapData;
            base.SetKeyBitmap(keyId, bitmapData);
        }
    }
}
