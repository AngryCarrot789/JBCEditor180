namespace BCEdit180.Core.Editor.FileSystem.Zip {
    /// <summary>
    /// The base class for items stored in a .jar file
    /// </summary>
    public class BaseZipItemViewModel : BaseExplorerItemViewModel {
        /// <summary>
        /// The jar file that this file belongs in. This may equal <see cref="BaseExplorerItemViewModel.Parent"/> if
        /// this item is a root item in the .jar file
        /// </summary>
        public ZipFileViewModel OwnerZip { get; }

        private string fullZipPath;

        public string FullZipPath {
            get => this.fullZipPath;
            set {
                this.RaisePropertyChanged(ref this.fullZipPath, value);
                this.ZipFileName = ZipFileViewModel.GetFileName(value, out _);
                this.RaisePropertyChanged(nameof(this.ZipFileName));
            }
        }

        public string ZipFileName { get; private set; }

        public BaseZipItemViewModel(ZipFileViewModel ownerZip) {
            this.OwnerZip = ownerZip;
        }
    }
}