using System.Threading.Tasks;

namespace BCEdit180.Core.Editor.FileSystem {
    /// <summary>
    /// The base class for all files in a file explorer tree
    /// </summary>
    public class BaseExplorerItemViewModel : BaseViewModel {
        internal BaseExplorerItemViewModel parent;

        /// <summary>
        /// The parent file
        /// </summary>
        public BaseExplorerItemViewModel Parent => this.parent;

        public virtual bool IsRoot => this.parent == null;

        private bool isExpanded;
        private bool isExpanding;

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

        public BaseExplorerItemViewModel() {

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
        }

        protected virtual Task<bool> OnExpandAsync() {
            return Task.FromResult(false);
        }
    }
}