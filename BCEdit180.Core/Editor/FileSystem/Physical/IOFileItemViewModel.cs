namespace BCEdit180.Core.Editor.FileSystem.Physical {
    /// <summary>
    /// A class for a regular file, such as a text document
    /// </summary>
    public class IOFileItemViewModel : BaseIOFileItemViewModel {
        public IOFileItemViewModel(string filePath) {
            this.FilePath = filePath;
        }
    }
}