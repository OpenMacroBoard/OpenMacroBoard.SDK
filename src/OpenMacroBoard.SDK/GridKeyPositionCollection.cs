using System.Collections.Generic;
using System.Drawing;

namespace OpenMacroBoard.SDK
{
    /// <summary>
    /// Represents a grid-like keyboard layout for macro boards.
    /// </summary>
    public class GridKeyPositionCollection : KeyPositionCollection
    {
        /// <summary>
        /// Number of keys horizontal
        /// </summary>
        public int KeyCountX { get; }

        /// <summary>
        /// Number of keys vertical
        /// </summary>
        public int KeyCountY { get; }

        /// <summary>
        /// Width of a single key (in px)
        /// </summary>
        public int KeyWidth { get; }

        /// <summary>
        /// Height of a single key (in px)
        /// </summary>
        public int KeyHeight { get; }

        /// <summary>
        /// The distance (in px) between columns
        /// </summary>
        public int KeyDistanceX { get; }

        /// <summary>
        /// The distance (in px) between rows
        /// </summary>
        public int KeyDistanceY { get; }

        /// <summary>
        /// Creates a <see cref="GridKeyPositionCollection"/> based on a rectangular grid layout.
        /// </summary>
        /// <param name="xCount">Number of keys in the x-coordinate (horizontal)</param>
        /// <param name="yCount">Number of keys in the y-coordinate (vertical)</param>
        /// <param name="width">Key width (px)</param>
        /// <param name="height">Key height (px)</param>
        /// <param name="dx">Distance between keys in x-coordinate (px)</param>
        /// <param name="dy">Distance between keys in y-coordinate (px)</param>
        public GridKeyPositionCollection(int xCount, int yCount, int width, int height, int dx, int dy)
            : base(CreateKeyPositions(xCount, yCount, width, height, dx, dy))
        {
            KeyCountX = xCount;
            KeyCountY = yCount;
            KeyHeight = height;
            KeyWidth = width;
        }

        /// <summary>
        /// Creates a <see cref="GridKeyPositionCollection"/> based on a rectangular grid layout.
        /// </summary>
        /// <param name="xCount">Number of keys in the x-coordinate (horizontal)</param>
        /// <param name="yCount">Number of keys in the y-coordinate (vertical)</param>
        /// <param name="keySize">Square key size (px)</param>
        /// <param name="keyDistance">Distance between keys (px)</param>
        public GridKeyPositionCollection(int xCount, int yCount, int keySize, int keyDistance)
            : this(xCount, yCount, keySize, keySize, keyDistance, keyDistance)
        {

        }

        private static IEnumerable<Rectangle> CreateKeyPositions(int xCount, int yCount, int width, int height, int dx, int dy)
        {
            var kWidth = width + dx;
            var kHeight = height + dy;

            for (int y = 0; y < yCount; y++)
                for (int x = 0; x < xCount; x++)
                    yield return new Rectangle(kWidth * x, kHeight * y, width, height);
        }
    }
}
