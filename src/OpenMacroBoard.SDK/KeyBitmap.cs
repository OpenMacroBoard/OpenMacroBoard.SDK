using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace OpenMacroBoard.SDK
{
    /// <summary>
    /// Represents a bitmap that can be used as key images
    /// </summary>
    public sealed class KeyBitmap : IEquatable<KeyBitmap>, IKeyBitmapDataAccess
    {
        /// <summary>
        /// Byte order is B-G-R, and pixels are stored left-to-right and top-to-bottom
        /// </summary>
        private readonly byte[] rawBitmapData;

        private int? cachedHashCode = null;

        internal KeyBitmap(int width, int height, byte[] bitmapData)
        {
            Width = width;
            Height = height;
            rawBitmapData = bitmapData ?? throw new ArgumentNullException(nameof(bitmapData));
        }

        /// <summary>
        /// This property can be used to create new KeyBitmaps
        /// </summary>
        /// <remarks>
        /// <para>This property just serves as an anchor point for extension methods
        /// to create new <see cref="KeyBitmap"/> objects</para>
        /// </remarks>
        public static IKeyBitmapFactory Create { get; }

        /// <summary>
        /// Solid black bitmap
        /// </summary>
        /// <remarks>
        /// <para>If you need a black bitmap (for example to clear keys) use this property for better performance (in theory ^^)</para>
        /// </remarks>
        public static KeyBitmap Black { get; } = new(1, 1, Array.Empty<byte>());

        /// <summary>
        /// Gets the width of the bitmap.
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// Gets the height of the bitmap.
        /// </summary>
        public int Height { get; }

        bool IKeyBitmapDataAccess.IsEmpty
            => rawBitmapData.Length == 0;

        /// <summary>
        /// The == operator
        /// </summary>
        public static bool operator ==(KeyBitmap a, KeyBitmap b)
        {
            return Equals(a, b);
        }

        /// <summary>
        /// The != operator
        /// </summary>
        public static bool operator !=(KeyBitmap a, KeyBitmap b)
        {
            return !Equals(a, b);
        }

        /// <summary>
        /// Compares the content of two given <see cref="KeyBitmap"/>s
        /// </summary>
        /// <param name="a">KeyBitmap a</param>
        /// <param name="b">KeyBitmap b</param>
        /// <returns>Returns true of the <see cref="KeyBitmap"/>s are equal and false otherwise.</returns>
        public static bool Equals(KeyBitmap a, KeyBitmap b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            if (a is null)
            {
                return false;
            }

            if (b is null)
            {
                return false;
            }

            if (a.Width != b.Width)
            {
                return false;
            }

            if (a.Height != b.Height)
            {
                return false;
            }

            if (ReferenceEquals(a.rawBitmapData, b.rawBitmapData))
            {
                return true;
            }

            if (a.rawBitmapData is null)
            {
                return false;
            }

            if (b.rawBitmapData is null)
            {
                return false;
            }

            return a.rawBitmapData.SequenceEqual(b.rawBitmapData);
        }

        /// <summary>
        /// Compares the content of this <see cref="KeyBitmap"/> to another KeyBitmap
        /// </summary>
        /// <param name="other">The other <see cref="KeyBitmap"/></param>
        /// <returns>True if both bitmaps are equals and false otherwise.</returns>
        public bool Equals(KeyBitmap other)
        {
            return Equals(this, other);
        }

        /// <summary>
        /// Compares the content of this <see cref="KeyBitmap"/> to another object
        /// </summary>
        /// <param name="obj">The other object</param>
        /// <returns>Return true if the other object is a <see cref="KeyBitmap"/> and equal to this one. Returns false otherwise.</returns>
        public override bool Equals(object obj)
        {
            return Equals(this, obj as KeyBitmap);
        }

        /// <summary>
        /// Get the hash code for this object.
        /// </summary>
        /// <returns>The hash code</returns>
        [SuppressMessage("Minor Bug", "S2328:\"GetHashCode\" should not reference mutable fields", Justification = "False positive because the value is cached.")]
        public override int GetHashCode()
        {
            return cachedHashCode ??= CalculateObjectHash();
        }

        /// <inheritdoc />
        ReadOnlySpan<byte> IKeyBitmapDataAccess.GetData()
        {
            return new ReadOnlySpan<byte>(rawBitmapData);
        }

        private int CalculateObjectHash()
        {
            const int initalValue = 17;
            const int primeFactor = 23;
            const int imageSampleSize = 1000;

            unchecked
            {
                var hash = initalValue;
                hash = hash * primeFactor + Width;
                hash = hash * primeFactor + Height;

                if (rawBitmapData.Length == 0)
                {
                    return hash;
                }

                var stepSize = 1;
                if (rawBitmapData.Length > imageSampleSize)
                {
                    stepSize = rawBitmapData.Length / imageSampleSize;
                }

                for (var i = 0; i < rawBitmapData.Length; i += stepSize)
                {
                    hash *= 23;
                    hash += rawBitmapData[i];
                }

                return hash;
            }
        }
    }
}
