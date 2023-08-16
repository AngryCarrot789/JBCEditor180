using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace BCEdit180.Core.Editor.FileSystem.Zip {
    public class ZipFolderEntryViewModel : BaseZipItemViewModel, IZipFolder {
        private readonly ObservableCollection<BaseZipItemViewModel> items;

        public ReadOnlyObservableCollection<BaseZipItemViewModel> Items { get; }

        IReadOnlyList<BaseZipItemViewModel> IZipFolder.Items => this.Items;

        public ZipFolderEntryViewModel(ZipFileViewModel ownerZip) : base(ownerZip) {
            this.items = new ObservableCollection<BaseZipItemViewModel>();
            this.Items = new ReadOnlyObservableCollection<BaseZipItemViewModel>(this.items);
        }

        protected override Task<bool> OnExpandAsync() {
            return Task.FromResult(this.items.Count > 0);
        }

        public void AddSorted(BaseZipItemViewModel item) {
            int index = ZipFileViewModel.BinarySearchInsertIndex(this, item);
            this.InsertItem(index, item);
        }

        public void InsertItem(int index, BaseZipItemViewModel item) {
            item.parent = this;
            this.items.Insert(index, item);
            item.RaisePropertyChanged(nameof(item.Parent));
        }
    }
}