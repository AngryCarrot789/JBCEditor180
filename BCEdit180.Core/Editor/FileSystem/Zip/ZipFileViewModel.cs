using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Compression;
using System.Threading.Tasks;
using BCEdit180.Core.Editor.FileSystem.Physical;
using BCEdit180.Core.Utils;

namespace BCEdit180.Core.Editor.FileSystem.Zip {
    /// <summary>
    /// A class that stores information and the file structure for a .jar file
    /// </summary>
    public class ZipFileViewModel : BaseIOFileItemViewModel, IZipFolder {
        private readonly ObservableCollection<BaseZipItemViewModel> items;

        public ZipFileViewModel OwnerZip => this;

        public string FullZipPath => null;

        IReadOnlyList<BaseZipItemViewModel> IZipFolder.Items => this.Items;

        /// <summary>
        /// A collection of items in this .jar file
        /// </summary>
        public ReadOnlyObservableCollection<BaseZipItemViewModel> Items { get; }

        public ZipFileViewModel(string filePath) {
            this.items = new ObservableCollection<BaseZipItemViewModel>();
            this.Items = new ReadOnlyObservableCollection<BaseZipItemViewModel>(this.items);
            this.FilePath = filePath;

            using (ZipArchive archive = ZipFile.OpenRead(filePath)) {
                foreach (ZipArchiveEntry entry in archive.Entries) {
                    ProcessEntry(this, entry);
                }
            }
        }

        protected override Task<bool> OnExpandAsync() {
            return Task.FromResult(this.items.Count > 0);
        }

        public void AddSorted(BaseZipItemViewModel item) {
            int index = BinarySearchInsertIndex(this, item);
            this.InsertItem(index, item);
        }

        public void InsertItem(int index, BaseZipItemViewModel item) {
            item.parent = this;
            this.items.Insert(index, item);
            item.RaisePropertyChanged(nameof(item.Parent));
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

        public static int BinarySearchInsertIndex(IZipFolder folder, BaseZipItemViewModel newItem) {
            int left = 0;
            int right = folder.Items.Count - 1;

            while (left <= right) {
                int middle = left + (right - left) / 2;
                BaseZipItemViewModel middleItem = folder.Items[middle];

                // Custom comparison logic based on your sorting criteria
                int comparison = CompareItems(newItem, middleItem);

                if (comparison < 0)
                    right = middle - 1;
                else if (comparison > 0)
                    left = middle + 1;
                else
                    return middle; // Item with the same value already exists
            }

            return left; // Index to insert the new item
        }

        private static int CompareItems(BaseZipItemViewModel a, BaseZipItemViewModel b) {
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
        }

        public static ZipFolderEntryViewModel GetOrCreateFolder(IZipFolder folder, string name) {
            foreach (BaseZipItemViewModel item in folder.Items) {
                if (item is ZipFolderEntryViewModel && item.ZipFileName == name) {
                    return (ZipFolderEntryViewModel) item;
                }
            }

            string root = folder.FullZipPath;
            ZipFolderEntryViewModel f = new ZipFolderEntryViewModel(folder.OwnerZip) {
                FullZipPath = (root != null ? root + name : name) + "/"
            };

            f.AddSorted(f);
            return f;
        }

        public static ZipFileEntryViewModel CreateFile(IZipFolder folder, string name) {
            foreach (BaseZipItemViewModel item in folder.Items) {
                if (item is ZipFileEntryViewModel && item.ZipFileName == name) {
                    return (ZipFileEntryViewModel) item;
                }
            }

            string root = folder.FullZipPath;
            ZipFileEntryViewModel file = new ZipFileEntryViewModel(folder.OwnerZip) {
                FullZipPath = root != null ? (root + name) : name
            };

            int index = BinarySearchInsertIndex(folder, file);
            folder.InsertItem(index, file);
            return file;
        }
    }
}