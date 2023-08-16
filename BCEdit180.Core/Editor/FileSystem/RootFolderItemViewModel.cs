using System.Collections.ObjectModel;
using BCEdit180.Core.Editor.FileSystem.Physical;

namespace BCEdit180.Core.Editor.FileSystem {
    public class RootFolderItemViewModel : BaseExplorerItemViewModel {
        private readonly ObservableCollection<BaseExplorerItemViewModel> items;

        /// <summary>
        /// A collection of items in this .jar file
        /// </summary>
        public ReadOnlyObservableCollection<BaseExplorerItemViewModel> Items { get; }

        public RootFolderItemViewModel() {
            this.items = new ObservableCollection<BaseExplorerItemViewModel>();
            this.Items = new ReadOnlyObservableCollection<BaseExplorerItemViewModel>(this.items);
        }

        public void AddFile(BaseExplorerItemViewModel item) {
            item.parent = this;
            this.items.Add(item);
            item.RaisePropertyChanged(nameof(item.Parent));
        }
    }
}