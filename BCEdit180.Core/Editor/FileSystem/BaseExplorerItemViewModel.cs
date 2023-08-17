using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BCEdit180.Core.Utils;

namespace BCEdit180.Core.Editor.FileSystem {
    /// <summary>
    /// The base class for all files in a file explorer tree
    /// </summary>
    public abstract class BaseExplorerItemViewModel : BaseViewModel {
        internal BaseExplorerItemViewModel parent;
        private FileExplorerViewModel explorer;

        /// <summary>
        /// The parent file
        /// </summary>
        public BaseExplorerItemViewModel Parent => this.parent;

        private bool isExpanded;
        private bool isExpanding;

        /// <summary>
        /// Whether or not this item has been expanded at least once by the user
        /// </summary>
        public bool HasExpandedOnce { get; private set; }

        /// <summary>
        /// Whether or not this item is currently expanded
        /// </summary>
        public bool IsExpanded {
            get => this.isExpanded;
            set {
                if (this.isExpanded == value) {
                    return;
                }

                if (this.isExpanding) {
                    this.RaisePropertyChanged(ref this.isExpanded, false);
                    return;
                }

                if (value) {
                    this.OnExpandInternal();
                }
                else {
                    this.RaisePropertyChanged(ref this.isExpanded, false);
                }
            }
        }

        public FileExplorerViewModel Explorer {
            get => this.explorer;
            private set => this.RaisePropertyChanged(ref this.explorer, value);
        }

        // use static versions to save memory

        public static AsyncRelayCommand<BaseExplorerItemViewModel> RefreshCommand { get; } = new AsyncRelayCommand<BaseExplorerItemViewModel>(x => x?.RefreshAsync() ?? Task.CompletedTask);
        public static AsyncRelayCommand<BaseExplorerItemViewModel> RemoveSelfCommand { get; } = new AsyncRelayCommand<BaseExplorerItemViewModel>((x) => {
            if (x?.Parent is IExplorerFolder folder)
                folder.RemoveItem(x);
            return Task.CompletedTask;
        });

        protected BaseExplorerItemViewModel() {

        }

        /// <summary>
        /// Sets this file's explorer. This should be overridden by files that contain child files to form a recursive set operation
        /// </summary>
        /// <param name="newExplorer"></param>
        public virtual void SetExplorer(FileExplorerViewModel newExplorer) {
            this.Explorer = newExplorer;
        }

        private async void OnExpandInternal() {
            this.isExpanding = true;
            try {
                this.isExpanded = await this.OnExpandAsync();
            }
            finally {
                this.isExpanding = false;
            }

            this.RaisePropertyChanged(nameof(this.IsExpanded));
            if (!this.HasExpandedOnce) {
                this.HasExpandedOnce = true;
                this.RaisePropertyChanged(nameof(this.HasExpandedOnce));
            }
        }

        protected virtual Task<bool> OnExpandAsync() {
            return Task.FromResult(false);
        }

        public static void AddItem<T>(BaseExplorerItemViewModel parent, IList<T> items, int index, T item) where T : BaseExplorerItemViewModel {
            if (item == parent)
                throw new Exception("Cannot add a parent to itself");
            if (items.Contains(item))
                throw new Exception("List already contains the item");

            item.parent = parent;
            items.Insert(index, item);
            item.SetExplorer(parent.Explorer);
            item.RaisePropertyChanged(nameof(item.Parent));
        }

        public static void RemoveItemAt<T>(IList<T> items, int index) where T : BaseExplorerItemViewModel {
            T item = items[index];
            item.OnRemovingFromParent();
            item.parent = null;
            item.Explorer = null;
            items.RemoveAt(index);
            item.RaisePropertyChanged(nameof(item.Parent));
        }

        public static bool RemoveItem<T>(IList<T> items, T item) where T : BaseExplorerItemViewModel {
            int index = items.IndexOf(item);
            if (index == -1)
                return false;
            RemoveItemAt(items, index);
            return true;
        }

        public static void ClearItems<T>(IList<T> items) where T : BaseExplorerItemViewModel {
            Exception ex = null;
            foreach (T item in items) {
                try {
                    item.OnRemovingFromParent();
                    item.parent = null;
                    item.Explorer = null;
                    item.RaisePropertyChanged(nameof(item.Parent));
                }
                catch (Exception e) {
                    ex = e;
                }
            }

            try {
                items.Clear();
            }
            catch (Exception e) {
                if (ex == null) {
                    ex = e;
                }
                else {
                    ex.AddSuppressed(e);
                }
            }

            if (ex != null) {
                throw ex;
            }
        }

        /// <summary>
        /// Refreshes this item, causing any data to be reloaded
        /// </summary>
        public virtual Task RefreshAsync() {
            return Task.CompletedTask;
        }

        public virtual void OnRemovingFromParent() {
            if (this is IExplorerFolder) {
                ((IExplorerFolder) this).Clear();
            }
        }
    }
}