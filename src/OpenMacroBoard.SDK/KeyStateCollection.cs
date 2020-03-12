using System.Collections;
using System.Collections.Generic;

namespace OpenMacroBoard.SDK
{
    /// <summary>
    /// A collection of booleans that indicate the current key states (true if pressed, false otherwise)
    /// </summary>
    public class KeyStateCollection : IKeyStateCollection
    {
        private readonly bool[] keyStates;

        /// <summary>
        /// Creates a new <see cref="KeyStateCollection"/> object
        /// </summary>
        /// <param name="keyStates">The data that is wrapped by this interface</param>
        public KeyStateCollection(bool[] keyStates)
        {
            this.keyStates = keyStates ?? throw new System.ArgumentNullException(nameof(keyStates));
        }

        /// <summary>
        /// Gets the number of keys in this collection
        /// </summary>
        public int KeyCount
            => keyStates.Length;

        /// <summary>
        /// Gets and sets the current state for a given key index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool this[int index]
            => keyStates[index];

        /// <summary>
        /// Enumerates all key states
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<int, bool>> GetEnumerator()
        {
            for (var i = 0; i < keyStates.Length; i++)
            {
                yield return new KeyValuePair<int, bool>(i, keyStates[i]);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
