namespace BCEdit180.Core.Editor.FileSystem {
    public class FileExplorerViewModel {
        public RootFolderItemViewModel Root { get; }

        public FileExplorerViewModel() {
            this.Root = new RootFolderItemViewModel();
        }
    }
}