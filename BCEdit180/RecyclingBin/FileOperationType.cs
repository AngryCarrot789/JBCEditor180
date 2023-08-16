namespace BCEdit180.RecyclingBin {
    /// <summary>
    /// File Operation Function Type for SHFileOperation
    /// </summary>
    public enum FileOperationType : uint {
        /// <summary>
        /// Move the objects
        /// </summary>
        FO_MOVE = 0x0001,

        /// <summary>
        /// Copy the objects
        /// </summary>
        FO_COPY = 0x0002,

        /// <summary>
        /// Delete the objects
        /// </summary>
        FO_DELETE = 0x0003,

        /// <summary>
        /// Rename the objects
        /// </summary>
        FO_RENAME = 0x0004,
    }
}