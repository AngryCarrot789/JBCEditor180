using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BCEdit180.Core.Editor.FileSystem {
    public class RootFolderItemViewModel : BaseExplorerItemViewModel, IExplorerFolder {
        private readonly ObservableCollection<BaseExplorerItemViewModel> items;

        /// <summary>
        /// A collection of items in this .jar file
        /// </summary>
        public ReadOnlyObservableCollection<BaseExplorerItemViewModel> Items { get; }

        IEnumerable<BaseExplorerItemViewModel> IExplorerFolder.Items => this.Items;

        public RootFolderItemViewModel() {
            this.items = new ObservableCollection<BaseExplorerItemViewModel>();
            this.Items = new ReadOnlyObservableCollection<BaseExplorerItemViewModel>(this.items);
        }

        public override void SetExplorer(FileExplorerViewModel newExplorer) {
            base.SetExplorer(newExplorer);
            foreach (BaseExplorerItemViewModel item in this.items) {
                item.SetExplorer(newExplorer);
            }
        }

        public void AddFile(BaseExplorerItemViewModel item) => AddItem(this, this.items, this.items.Count, item);

        public bool RemoveItem(BaseExplorerItemViewModel item) => RemoveItem(this.items, item);

        public void RemoveItemAt(int index) => RemoveItemAt(this.items, index);

        public void Clear() => ClearItems(this.items);
    }
}