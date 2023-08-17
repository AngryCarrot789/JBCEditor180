using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using BCEdit180.Core.Editor.FileSystem.Zip;
using BCEdit180.Core.Utils;

namespace BCEdit180.Core.Editor.FileSystem.Physical {
    public class IOFolderItemViewModel : BaseIOFileItemViewModel, IExplorerFolder {
        private readonly ObservableCollection<BaseIOFileItemViewModel> items;

        /// <summary>
        /// A collection of items in this .jar file
        /// </summary>
        public ReadOnlyObservableCollection<BaseIOFileItemViewModel> Items { get; }

        IEnumerable<BaseExplorerItemViewModel> IExplorerFolder.Items => this.Items;

        private bool hasDummyItem;

        public IOFolderItemViewModel(string filePath) {
            this.items = new ObservableCollection<BaseIOFileItemViewModel>();
            this.Items = new ReadOnlyObservableCollection<BaseIOFileItemViewModel>(this.items);
            this.hasDummyItem = true;
            this.items.Add(null);

            this.FilePath = filePath;
        }

        public override void SetExplorer(FileExplorerViewModel newExplorer) {
            base.SetExplorer(newExplorer);
            if (!this.hasDummyItem) {
                foreach (BaseIOFileItemViewModel item in this.items) {
                    item.SetExplorer(newExplorer);
                }
            }
        }

        protected override async Task<bool> OnExpandAsync() {
            if (this.hasDummyItem) {
                this.Clear();
                if (!Directory.Exists(this.FilePath)) {
                    return false;
                }

                await this.QueryFilesFromSystem();
            }

            return this.Items.Count > 0;
        }

        private async Task QueryFilesFromSystem() {
            DirectoryInfo info = new DirectoryInfo(this.FilePath);
            IEnumerable<FileSystemInfo> enumerable;
            try {
                enumerable = info.EnumerateFileSystemInfos();
            }
            catch (UnauthorizedAccessException e) {
                await IoC.MessageDialogs.ShowMessageExAsync("Unauthorized Access", "Cannot access this folder", e.GetToString());
                return;
            }

            foreach (FileSystemInfo item in enumerable) {
                if (item is DirectoryInfo directory) {
                    this.AddFile(new IOFolderItemViewModel(directory.FullName));
                }
                else {
                    FileInfo file = (FileInfo) item;
                    string extension = file.Extension;
                    if (extension == ".jar" || extension == ".zip") {
                        this.AddFile(new ZipFileViewModel(file.FullName));
                    }
                    else {
                        this.AddFile(new IOFileItemViewModel(file.FullName));
                    }
                }
            }
        }

        private static readonly Comparison<BaseIOFileItemViewModel> SortComparer = (a, b) => {
            if (a is IOFolderItemViewModel) {
                if (b is IOFolderItemViewModel) {
                    return string.Compare(a.FileName, b.FileName);
                }
                else {
                    return -1; // A comes before B
                }
            }
            else if (b is IOFolderItemViewModel) {
                return 1; // A comes after B
            }
            else {
                return string.Compare(a.FileName, b.FileName);
            }
        };

        public void AddFile(BaseIOFileItemViewModel item) {
            if (this.hasDummyItem) {
                this.hasDummyItem = false;
                this.items.Clear();
            }

            int index = CollectionUtils.GetSortInsertionIndex(this.Items, item, SortComparer);
            AddItem(this, this.items, index, item);
        }

        public bool RemoveItem(BaseExplorerItemViewModel item) {
            if (!(item is BaseIOFileItemViewModel))
                return false;
            int index = this.items.IndexOf((BaseIOFileItemViewModel) item);
            if (index == -1)
                return false;
            RemoveItemAt(this.items, index);
            return true;
        }

        public void RemoveItemAt(int index) => RemoveItemAt(this.items, index);

        public void Clear() {
            if (this.hasDummyItem) {
                this.hasDummyItem = false;
            }
            else {
                for (int i = this.items.Count - 1; i >= 0; i--) {
                    RemoveItemAt(this.items, i);
                }
            }

            this.items.Clear();
        }

        public override async Task RefreshAsync() {
            if (Directory.Exists(this.FilePath)) {
                this.Clear();
                await this.QueryFilesFromSystem();
            }
            else if (this.parent != null) {
                await this.parent.RefreshAsync();
            }
        }
    }
}