using System.Collections.Generic;

namespace BCEdit180.Core.Editor.FileSystem {
    /// <summary>
    /// An interface for classes that store child files, and where files can be removed
    /// </summary>
    public interface IExplorerFolder {
        IEnumerable<BaseExplorerItemViewModel> Items { get; }

        /// <summary>
        /// Tries to remove an item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        bool RemoveItem(BaseExplorerItemViewModel item);

        /// <summary>
        /// Removes an item at the specific index
        /// </summary>
        /// <param name="index"></param>
        void RemoveItemAt(int index);

        /// <summary>
        /// Recursively clears this folder
        /// </summary>
        void Clear();
    }
}