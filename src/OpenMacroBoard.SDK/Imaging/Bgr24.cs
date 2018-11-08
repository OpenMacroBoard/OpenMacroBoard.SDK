namespace OpenMacroBoard.SDK.Imaging
{
    /// <summary>
    /// A <see cref="IPixelFormat"/> that is three bytes in size.
    /// Blue, red, green (in that order) represented as a single byte channel.
    /// </summary>
    public sealed class Bgr24 : IPixelFormat
    {
        /// <inheritdoc/>
        public int BytesPerPixel { get; } = 3;

        internal Bgr24() { }

        /// <inheritdoc/>
        public override bool Equals(object obj)
            => PixelFormats.Equals(this, obj as IPixelFormat);

        /// <inheritdoc/>
        public override int GetHashCode()
            => GetType().GetHashCode();
    }
}
