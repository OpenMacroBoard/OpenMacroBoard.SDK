namespace OpenMacroBoard.SDK.Imaging
{
    public static class PixelFormats
    {
        public static Bgr24 Bgr24 { get; } = new Bgr24();
        public static Bgra32 Bgra32 { get; } = new Bgra32();

        public static bool Equals(IPixelFormat a, IPixelFormat b)
            => ReferenceEquals(a, b);
    }
}
