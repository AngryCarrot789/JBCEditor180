using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace BCEdit180.Core.Editor.FileSystem.Zip {
    public class ZipFolderEntryViewModel : BaseZipItemViewModel, IZipFolder {
        private readonly ObservableCollection<BaseZipItemViewModel> items;

        public ReadOnlyObservableCollection<BaseZipItemViewModel> Items { get; }

        IReadOnlyList<BaseZipItemViewModel> IZipFolder.ZipItems => this.Items;

        IEnumerable<BaseExplorerItemViewModel> IExplorerFolder.Items => this.Items;

        public ZipFolderEntryViewModel(ZipFileViewModel ownerZip) : base(ownerZip) {
            this.items = new ObservableCollection<BaseZipItemViewModel>();
            this.Items = new ReadOnlyObservableCollection<BaseZipItemViewModel>(this.items);
        }

        public override void SetExplorer(FileExplorerViewModel newExplorer) {
            base.SetExplorer(newExplorer);
            foreach (BaseZipItemViewModel item in this.items) {
                item.SetExplorer(newExplorer);
            }
        }

        protected override Task<bool> OnExpandAsync() {
            return Task.FromResult(this.items.Count > 0);
        }

        public void AddZipItemSorted(BaseZipItemViewModel item) {
            this.InsertItem(ZipFileViewModel.BinarySearchInsertIndex(this, item), item);
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

        public void Clear() {
            foreach (BaseZipItemViewModel item in this.items) {
                if (item is IExplorerFolder folder) {
                    folder.Clear();
                }
            }

            this.items.Clear();
        }
    }
}