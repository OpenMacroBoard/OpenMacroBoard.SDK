using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace OpenMacroBoard.SDK
{
    /// <summary>
    /// Represents a keyboard layout for macro boards by representing all LCD keys
    /// as a collection of rectangles with their position on the board
    /// </summary>
    /// <remarks>
    /// This structure allows OpenMacroBoard to support complex layouts (eq. optimus maximus).
    /// </remarks>
    public class KeyPositionCollection : IKeyPositionCollection
    {
        private readonly Rectangle[] keyPositions;

        /// <summary>
        /// Creates a <see cref="KeyPositionCollection"/>.
        /// </summary>
        /// <param name="keyPositions"></param>
        public KeyPositionCollection(IEnumerable<Rectangle> keyPositions)
        {
            this.keyPositions = keyPositions.ToArray();
            VerifyKeyPositionData(this.keyPositions);

            Area = GetFullArea(this.keyPositions);
        }

        /// <summary>
        /// Enumerates all keys
        /// </summary>
        /// <returns></returns>
        public IEnumerator<Rectangle> GetEnumerator()
        {
            for (int i = 0; i < keyPositions.Length; i++)
                yield return keyPositions[i];
        }

        /// <summary>
        /// Gets a key position with a given index.
        /// </summary>
        /// <param name="keyIndex"></param>
        /// <returns></returns>
        public Rectangle this[int keyIndex]
            => keyPositions[keyIndex];

        /// <summary>
        /// The number of keys
        /// </summary>
        public int Count
            => keyPositions.Length;

        /// <summary>
        /// The smallest area that contains all keys
        /// </summary>
        /// <remarks>
        /// This can be used for example to create full screen images that span over all keys
        /// </remarks>
        public Rectangle Area { get; }

        private static void VerifyKeyPositionData(IEnumerable<Rectangle> rectangles)
        {
            foreach (var r in rectangles)
            {
                if (r.Width <= 0 || r.Height <= 0)
                    throw new ArgumentException("Height and Width must be ≥ 1");

                if (r.Left < 0 || r.Top < 0)
                    throw new ArgumentException("All key positions must be positive");
            }
        }

        private static Rectangle GetFullArea(IEnumerable<Rectangle> rectangles)
        {
            var minX = 0;
            var minY = 0;
            var maxX = 0;
            var maxY = 0;

            foreach (var kp in rectangles)
            {
                minX = Math.Min(minX, kp.Left);
                minY = Math.Min(minY, kp.Top);
                maxX = Math.Max(maxX, kp.Right);
                maxY = Math.Max(maxY, kp.Bottom);
            }

            return new Rectangle(minX, minY, maxX - minX, maxY - minY);
        }

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
