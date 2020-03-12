using System;

namespace OpenMacroBoard.SDK
{
    /// <summary>
    /// An interface that enables communication with a specific key range
    /// </summary>
    public interface IMacroKeyArea
    {
        /// <summary>
        /// Informations about the keys and their position
        /// </summary>
        IKeyPositionCollection Keys { get; }

        /// <summary>
        /// Is raised when a key is pressed
        /// </summary>
        event EventHandler<KeyEventArgs> KeyStateChanged;

        /// <summary>
        /// The current state of keys (true if pressed, false otherwise)
        /// </summary>
        IKeyStateCollection KeyStates { get; }

        /// <summary>
        /// Sets a background image for a given key
        /// </summary>
        /// <param name="keyId">Specifies which key the image will be applied on</param>
        /// <param name="bitmapData">Bitmap. The key will be painted black if this value is null.</param>
        void SetKeyBitmap(int keyId, KeyBitmap bitmapData);
    }
}
