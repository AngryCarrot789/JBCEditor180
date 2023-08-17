using System.Threading.Tasks;
using BCEdit180.Core.Editor.FileSystem.Events;
using BCEdit180.Core.Editor.FileSystem.Physical;
using BCEdit180.Core.Editor.FileSystem.Zip;

namespace BCEdit180.Core.Editor.FileSystem {
    public class FileExplorerViewModel {
        public RootFolderItemViewModel Root { get; }

        public AsyncRelayCommand<BaseExplorerItemViewModel> OpenItemCommand { get; }

        public event OpenFileEventHandler OpenFile;
        public event OpenZipEntryEventHandler OpenZipFile;

        public FileExplorerViewModel() {
            this.OpenItemCommand = new AsyncRelayCommand<BaseExplorerItemViewModel>(this.OpenFileAction);
            this.Root = new RootFolderItemViewModel();
            this.Root.SetExplorer(this);
        }

        public async Task OpenFileAction(BaseExplorerItemViewModel item) {
            if (item is IOFileItemViewModel) {
                await this.OpenFileAsync((IOFileItemViewModel) item);
            }
            else if (item is ZipFileEntryViewModel) {
                await this.OpenFileAsync((ZipFileEntryViewModel) item);
            }
            else {
                await IoC.MessageDialogs.ShowDialogAsync("Cannot open", "This file cannot be opened. Must be a physical file or zip file");
            }
        }

        // Are task events bad?

        /// <summary>
        /// Called by IO files when they are trying to be opened
        /// </summary>
        /// <param name="file"></param>
        /// <exception cref="NotImplementedException"></exception>
        public async Task OpenFileAsync(IOFileItemViewModel file) {
            OpenFileEventHandler x = this.OpenFile;
            if (x != null)
                await x(file);
        }

        public async Task OpenFileAsync(ZipFileEntryViewModel file) {
            OpenZipEntryEventHandler x = this.OpenZipFile;
            if (x != null)
                await x(file);
        }
    }
}