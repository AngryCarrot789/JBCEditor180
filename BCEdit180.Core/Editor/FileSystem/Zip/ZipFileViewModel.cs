using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Compression;
using System.Threading.Tasks;
using BCEdit180.Core.Editor.FileSystem.Physical;
using BCEdit180.Core.Utils;

namespace BCEdit180.Core.Editor.FileSystem.Zip {
    /// <summary>
    /// A class that stores information and the file structure for a .zip or .jar file
    /// </summary>
    public class ZipFileViewModel : BaseIOFileItemViewModel, IZipFolder {
        private readonly ObservableCollection<BaseZipItemViewModel> items;

        public ZipFileViewModel OwnerZip => this;

        public string FullZipPath => null;

        IReadOnlyList<BaseZipItemViewModel> IZipFolder.ZipItems => this.Items;

        IEnumerable<BaseExplorerItemViewModel> IExplorerFolder.Items => this.Items;

        /// <summary>
        /// A collection of items in this .jar file
        /// </summary>
        public ReadOnlyObservableCollection<BaseZipItemViewModel> Items { get; }

        public ZipArchive Archive { get; private set; }

        public ZipFileViewModel(string filePath) {
            this.items = new ObservableCollection<BaseZipItemViewModel>();
            this.Items = new ReadOnlyObservableCollection<BaseZipItemViewModel>(this.items);
            this.FilePath = filePath;
        }

        ~ZipFileViewModel() {
            this.Archive?.Dispose();
        }

        public override void SetExplorer(FileExplorerViewModel newExplorer) {
            base.SetExplorer(newExplorer);
            foreach (BaseZipItemViewModel item in this.items) {
                item.SetExplorer(newExplorer);
            }
        }

        protected override async Task<bool> OnExpandAsync() {
            // Dynamically load zip file to reduce RAM when a zip is never explored
            if (this.Archive == null) {
                try {
                    this.Archive = ZipFile.OpenRead(this.FilePath);
                }
                catch (Exception e) {
                    await IoC.MessageDialogs.ShowMessageExAsync("Zip Failure", "Failed to read zip/jar file", e.GetToString());
                    return false;
                }

                foreach (ZipArchiveEntry entry in this.Archive.Entries) {
                    ProcessEntry(this, entry);
                }
            }

            return this.items.Count > 0;
        }

        public void AddZipItemSorted(BaseZipItemViewModel item) {
            this.InsertItem(BinarySearchInsertIndex(this, item), item);
        }

        public void InsertItem(int index, BaseZipItemViewModel item) {
            AddItem(this, this.items, index, item);
        }

        public bool RemoveItem(BaseExplorerItemViewModel item) {
            if (!(item is BaseZipItemViewModel))
                return false;
            int index = this.items.IndexOf((BaseZipItemViewModel) item);
            if (index == -1)
                return false;
            RemoveItemAt(this.items, index);
            return true;
        }

        public void RemoveItemAt(int index) {
            RemoveItemAt(this.items, index);
        }

        public void Clear() => ClearItems(this.items);

        public override void OnRemovingFromParent() {
            base.OnRemovingFromParent();
            this.Archive?.Dispose();
            GC.SuppressFinalize(this);
        }

        public static string GetFileName(string path, out bool isDirectory) {
            isDirectory = path[path.Length - 1] == '/';
            int lastIndex = path.LastIndexOf('/', path.Length - (isDirectory ? 2 : 1));
            if (lastIndex == -1) {
                return isDirectory ? path.Substring(0, path.Length - 1) : path;
            }
            else {
                return path.JSubstring(lastIndex + 1, path.Length - (isDirectory ? 1 : 0));
            }
        }

        public static void ProcessEntry(IZipFolder folder, ZipArchiveEntry entry) {
            // TODO: Heavily optimised; i'm lazy and cba to implement a more efficient version LOL

            // reghzy/app/
            // reghzy/app/okay/
            // reghzy/app/hi.png
            IZipFolder next = folder;
            string[] split = entry.FullName.Split('/');
            int c = split.Length - 1;
            for (int i = 0; i < c; i++) {
                next = GetOrCreateFolder(next, split[i]);
            }

            if (c >= 0 && !string.IsNullOrEmpty(split[c])) {
                CreateFile(next, split[split.Length - 1]);
            }
        }

        private static readonly Comparison<BaseZipItemViewModel> SortComparer = (a, b) => {
            if (a is ZipFolderEntryViewModel) {
                if (b is ZipFolderEntryViewModel) {
                    return string.Compare(a.ZipFileName, b.ZipFileName);
                }
                else {
                    return -1; // A comes before B
                }
            }
            else if (b is ZipFolderEntryViewModel) {
                return 1; // A comes after B
            }
            else {
                return string.Compare(a.ZipFileName, b.ZipFileName);
            }
        };

        public static int BinarySearchInsertIndex(IZipFolder folder, BaseZipItemViewModel newItem) {
            return CollectionUtils.GetSortInsertionIndex(folder.ZipItems, newItem, SortComparer);
        }

        public static ZipFolderEntryViewModel GetOrCreateFolder(IZipFolder container, string name) {
            foreach (BaseZipItemViewModel item in container.ZipItems) {
                if (item is ZipFolderEntryViewModel && item.ZipFileName == name) {
                    return (ZipFolderEntryViewModel) item;
                }
            }

            string root = container.FullZipPath;
            ZipFolderEntryViewModel f = new ZipFolderEntryViewModel(container.OwnerZip) {
                FullZipPath = (root != null ? root + name : name) + "/"
            };

            container.AddZipItemSorted(f);
            return f;
        }

        public static ZipFileEntryViewModel CreateFile(IZipFolder container, string name) {
            foreach (BaseZipItemViewModel item in container.ZipItems) {
                if (item is ZipFileEntryViewModel && item.ZipFileName == name) {
                    return (ZipFileEntryViewModel) item;
                }
            }

            string root = container.FullZipPath;
            ZipFileEntryViewModel file = new ZipFileEntryViewModel(container.OwnerZip) {
                FullZipPath = root != null ? (root + name) : name
            };

            container.AddZipItemSorted(file);
            return file;
        }

        public ZipArchiveEntry GetEntry(string entryName) {
            return this.Archive?.GetEntry(entryName);
        }
    }
}