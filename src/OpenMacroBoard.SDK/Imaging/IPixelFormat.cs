namespace OpenMacroBoard.SDK.Imaging
{
    /// <summary>
    /// Represents a pixel format for a <see cref="BitmapData"/> object.
    /// </summary>
    public interface IPixelFormat
    {
        /// <summary>
        /// Gets the number of bytes per pixel
        /// </summary>
        int BytesPerPixel { get; }
    }
}
