using System.Collections.Generic;

namespace BCEdit180.Core.Editor.FileSystem.Zip {
    public interface IZipFolder : IExplorerFolder {
        /// <summary>
        /// The owner zip archive file. May return the current instance
        /// </summary>
        ZipFileViewModel OwnerZip { get; }

        /// <summary>
        /// Gets the folder's path within the zip archive, or null if this folder is the root zip archive. Regular files and folders will not have null paths
        /// </summary>
        string FullZipPath { get; }

        /// <summary>
        /// Returns the zip items in this folder
        /// </summary>
        IReadOnlyList<BaseZipItemViewModel> ZipItems { get; }

        /// <summary>
        /// Adds the file to this collection
        /// </summary>
        /// <param name="file"></param>
        void AddZipItemSorted(BaseZipItemViewModel file);
    }
}