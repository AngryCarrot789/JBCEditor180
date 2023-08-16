using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using BCEdit180.Core.Editor.FileSystem.Zip;

namespace BCEdit180.Core.Editor.FileSystem.Physical {
    public class IOFolderItemViewModel : BaseIOFileItemViewModel {
        private readonly ObservableCollection<BaseIOFileItemViewModel> items;

        /// <summary>
        /// A collection of items in this .jar file
        /// </summary>
        public ReadOnlyObservableCollection<BaseIOFileItemViewModel> Items { get; }

        private bool hasDummyItem;

        public IOFolderItemViewModel(string filePath) {
            this.items = new ObservableCollection<BaseIOFileItemViewModel>();
            this.Items = new ReadOnlyObservableCollection<BaseIOFileItemViewModel>(this.items);
            this.hasDummyItem = true;
            this.items.Add(null);

            this.FilePath = filePath;
        }

        protected override async Task<bool> OnExpandAsync() {
            if (this.hasDummyItem) {
                this.Clear();
                if (!Directory.Exists(this.FilePath)) {
                    return false;
                }

                foreach (string item in Directory.EnumerateFileSystemEntries(this.FilePath)) {
                    if (Directory.Exists(item)) {
                        this.AddFile(new IOFolderItemViewModel(item));
                    }
                    else if (File.Exists(item)) {
                        string extension = Path.GetExtension(item);
                        if (extension == ".jar" || extension == ".zip") {
                            this.AddFile(new ZipFileViewModel(item));
                        }
                        else {
                            this.AddFile(new IOFileItemViewModel(item));
                        }
                    }
                }
            }

            return this.Items.Count > 0;
        }

        public void AddFile(BaseIOFileItemViewModel item) {
            item.parent = this;
            this.items.Add(item);
            item.RaisePropertyChanged(nameof(item.Parent));
        }

        public void Clear() {
            if (this.hasDummyItem) {
                this.hasDummyItem = false;
            }
            else {
                foreach (BaseIOFileItemViewModel item in this.items) {
                    item.parent = null;
                    item.RaisePropertyChanged(nameof(item.Parent));
                }
            }

            this.items.Clear();
        }
    }
}