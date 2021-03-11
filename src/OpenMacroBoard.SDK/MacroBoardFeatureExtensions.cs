using System;

namespace OpenMacroBoard.SDK
{
    /// <summary>
    /// Extensions for <see cref="IMacroBoard"/> enrichment ;-)
    /// </summary>
    public static class MacroBoardFeatureExtensions
    {
        /// <summary>
        /// Wraps an <see cref="IMacroBoard"/> with an button press effect adapter.
        /// </summary>
        /// <param name="macroBoard">The board that should be wrapped.</param>
        /// <param name="config">The configuration that should be used. Changes to the configuration later also takes effect.</param>
        /// <returns>Returns a new board that implements the button press effect.</returns>
        public static IMacroBoard WithButtonPressEffect(this IMacroBoard macroBoard, ButtonPressEffectConfig config = null)
        {
            if (macroBoard is null)
            {
                throw new ArgumentNullException(nameof(macroBoard));
            }

            return new ButtonPressEffectAdapter(macroBoard, config);
        }

        /// <summary>
        /// Wraps an <see cref="IMacroBoard"/> with a disconnect replay adapter.
        /// </summary>
        /// <param name="macroBoard">The board that should be wrapped.</param>
        /// <returns>Returns a new board that implements the replay feature.</returns>
        /// <remarks>
        /// This adapter makes sure, that if a device is disconnected that previously set properties like
        /// images and brightness are replayed if the device is connected again.
        /// </remarks>
        public static IMacroBoard WithDisconnectReplay(this IMacroBoard macroBoard)
        {
            if (macroBoard is null)
            {
                throw new ArgumentNullException(nameof(macroBoard));
            }

            return new DisconnectReplayAdapter(macroBoard);
        }
    }
}
