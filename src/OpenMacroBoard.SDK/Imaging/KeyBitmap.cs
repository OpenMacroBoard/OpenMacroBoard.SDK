using System;
using System.Linq;

namespace OpenMacroBoard.SDK.Imaging
{
    /// <summary>
    /// Contains immutable raw bitmap data with a given format.
    /// </summary>
    public sealed class KeyBitmap : IEquatable<KeyBitmap>
    {
        /// <summary>
        /// This property can be used to create new KeyBitmaps
        /// </summary>
        /// <remarks>
        /// This property just serves as an anchor point for extension methods
        /// to create new <see cref="KeyBitmap"/> objects
        /// </remarks>
        public static IKeyBitmapFactory Create { get; }

        /// <summary>
        /// Solid black bitmap
        /// </summary>
        /// <remarks>
        /// If you need a black bitmap (for example to clear keys) use this property for better performance (in theory ^^)
        /// </remarks>
        public static KeyBitmap Black { get; } = new KeyBitmap(1, 1, 3, PixelFormats.Bgr24);

        /// <summary>
        /// Gets the pixel width of the bitmap
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// Gets the pixel height of the bitmap
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// Gets the stride (in bytes)
        /// </summary>
        public int Stride { get; }

        /// <summary>
        /// Gets the number of raw bytes that represent this bitmap
        /// </summary>
        public int DataLength { get; }

        /// <summary>
        /// Gets the bitmap format used for this bitmap.
        /// </summary>
        public IPixelFormat Format { get; }

        /// <summary>
        /// Gets a specific byte of the bitmap data
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public byte this[int index]
        {
            get => data[index];
        }

        internal byte[] data;

        internal KeyBitmap(int width, int height, int stride, IPixelFormat format)
        {
            if (width <= 0)
                throw new ArgumentOutOfRangeException(nameof(width));

            if (height <= 0)
                throw new ArgumentOutOfRangeException(nameof(height));

            var minimumStride = width * format.BytesPerPixel;
            if (stride < minimumStride)
                throw new ArgumentOutOfRangeException(nameof(stride));

            Width = width;
            Height = height;
            Stride = stride;
            Format = format ?? throw new ArgumentNullException(nameof(format));

            data = new byte[stride * height];
            DataLength = data.Length;
        }

        /// <summary>
        /// Gets a value indicating wether <paramref name="a"/> and <paramref name="b"/> are equal.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>Returns true if equal and false otherwise</returns>
        public static bool Equals(KeyBitmap a, KeyBitmap b)
        {
            if (ReferenceEquals(a, b))
                return true;

            if (a is null)
                return false;

            if (b is null)
                return false;

            if (a.Width != b.Width)
                return false;

            if (a.Height != b.Height)
                return false;

            if (!ReferenceEquals(a.Format, b.Format))
            {
                if (a.Format is null)
                    return false;

                if (b.Format is null)
                    return false;

                if (!a.Format.Equals(b.Format))
                    return false;
            }

            if (!ReferenceEquals(a.data, b.data))
            {
                if (a.data is null)
                    return false;

                if (b.data is null)
                    return false;

                if (!a.data.SequenceEqual(b.data))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// The == operator
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(KeyBitmap a, KeyBitmap b)
            => Equals(a, b);

        /// <summary>
        /// The != operator
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(KeyBitmap a, KeyBitmap b)
            => !Equals(a, b);

        /// <inheritdoc />
        public bool Equals(KeyBitmap other)
            => Equals(this, other);

        /// <inheritdoc />
        public override bool Equals(object obj)
            => Equals(this, obj as KeyBitmap);

        private int hashCode = -1;

        /// <inheritdoc />
        public override int GetHashCode()
        {
            if (hashCode == -1)
            {
                var newHash = CalculateHashCode();
                hashCode = newHash == -1 ? 0 : newHash;
            }

            return hashCode;
        }

        private int CalculateHashCode()
        {
            const int initalValue = 17;
            const int primeFactor = 23;
            const int imageSampleSize = 1000;

            unchecked
            {
                var hash = initalValue;
                hash = hash * primeFactor + Width;
                hash = hash * primeFactor + Height;

                if (data == null)
                    return hash;

                var stepSize = 1;
                if (data.Length > imageSampleSize)
                    stepSize = data.Length / imageSampleSize;

                for (int i = 0; i < data.Length; i += stepSize)
                {
                    hash *= 23;
                    hash += data[i];
                }

                return hash;
            }
        }
    }
}
