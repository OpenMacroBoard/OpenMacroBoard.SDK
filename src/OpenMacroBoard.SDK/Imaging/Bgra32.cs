namespace OpenMacroBoard.SDK.Imaging
{
    /// <summary>
    /// A <see cref="IPixelFormat"/> that is four bytes in size.
    /// Blue, red, green, alpha (in that order) represented as a single byte channel.
    /// </summary>
    public sealed class Bgra32 : IPixelFormat
    {
        /// <inheritdoc/>
        public int BytesPerPixel { get; } = 4;

        internal Bgra32() { }

        /// <inheritdoc/>
        public override bool Equals(object obj)
            => PixelFormats.Equals(this, obj as IPixelFormat);

        /// <inheritdoc/>
        public override int GetHashCode()
            => GetType().GetHashCode();
    }
}
