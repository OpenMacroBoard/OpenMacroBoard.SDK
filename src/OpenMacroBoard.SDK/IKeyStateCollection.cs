using System.Collections.Generic;

namespace OpenMacroBoard.SDK
{
    /// <summary>
    /// A collection of the current key states
    /// </summary>
    public interface IKeyStateCollection : IEnumerable<KeyValuePair<int, bool>>
    {
        /// <summary>
        /// Gets the current key state for a given key index
        /// </summary>
        /// <param name="index">index of the key</param>
        /// <returns></returns>
        bool this[int index] { get; }
    }
}
