using System.IO;

namespace BCEdit180.Core.Editor.FileSystem.Physical {
    /// <summary>
    /// The base class for all IO file items that have a path on the operating system's file system
    /// </summary>
    public class BaseIOFileItemViewModel : BaseExplorerItemViewModel {
        private string filePath;
        public string FilePath {
            get => this.filePath;
            protected set {
                if (this.filePath == value) {
                    return;
                }

                this.RaisePropertyChanged(ref this.filePath, value);
                this.RaisePropertyChanged(nameof(this.FileName));
                this.OnFilePathChanged();
            }
        }

        public string FileName {
            get {
                if (string.IsNullOrWhiteSpace(this.FilePath))
                    return "";
                string name = Path.GetFileName(this.FilePath);
                return string.IsNullOrEmpty(name) ? this.FilePath : name;
            }
        }

        protected virtual void OnFilePathChanged() {

        }
    }
}