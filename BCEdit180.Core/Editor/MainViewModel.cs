using System.Threading.Tasks;
using BCEdit180.Core.Editor.FileSystem;
using BCEdit180.Core.Editor.FileSystem.Physical;

namespace BCEdit180.Core.Editor {
    public class MainViewModel : BaseViewModel {
        public AsyncRelayCommand OpenFolderCommand { get; }

        public FileExplorerViewModel Explorer { get; }

        public ClassManagerViewModel ClassManager { get; }

        public MainViewModel() {
            this.OpenFolderCommand = new AsyncRelayCommand(this.OpenFolderAction);
            this.Explorer = new FileExplorerViewModel();
            this.ClassManager = new ClassManagerViewModel(this);
        }

        private async Task OpenFolderAction() {
            string path = await IoC.FilePicker.OpenFolder(null, "Select a folder to open");
            if (string.IsNullOrEmpty(path)) {
                return;
            }

            IOFolderItemViewModel root = new IOFolderItemViewModel(path);
            this.Explorer.Root.AddFile(root);
        }
    }
}