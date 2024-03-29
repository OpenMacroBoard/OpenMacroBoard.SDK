using System;

namespace OpenMacroBoard.SDK
{
    /// <summary>
    /// An interface that allows you to access the underlying data of <see cref="KeyBitmap"/>s.
    /// </summary>
    public interface IKeyBitmapDataAccess
    {
        /// <summary>
        /// Gets the width of the bitmap.
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Gets the height of the bitmap.
        /// </summary>
        int Height { get; }

        /// <summary>
        /// Gets a value indicating whether the underlying byte array is null.
        /// </summary>
        bool IsEmpty { get; }

        /// <summary>
        /// Gets the underlying image data in unaligned Bgr24 format (stride = width * 3).
        /// </summary>
        ReadOnlySpan<byte> GetData();
    }
}
