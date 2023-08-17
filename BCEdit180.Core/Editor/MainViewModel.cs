using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using BCEdit180.Core.Editor.Classes;
using BCEdit180.Core.Editor.FileSystem;
using BCEdit180.Core.Editor.FileSystem.Physical;
using BCEdit180.Core.Editor.FileSystem.Zip;
using BCEdit180.Core.Utils;

namespace BCEdit180.Core.Editor {
    public class MainViewModel : BaseViewModel {
        public FileExplorerViewModel Explorer { get; }

        public ClassManagerViewModel ClassManager { get; }

        public AsyncRelayCommand OpenFolderCommand { get; }

        public MainViewModel() {
            this.OpenFolderCommand = new AsyncRelayCommand(this.OpenFolderAction);
            this.Explorer = new FileExplorerViewModel();
            this.ClassManager = new ClassManagerViewModel(this);

            this.Explorer.OpenFile += this.ExplorerOnOpenFile;
            this.Explorer.OpenZipFile += this.ExplorerOnOpenZipFile;
        }

        private async Task ExplorerOnOpenFile(IOFileItemViewModel file) {
            if (!File.Exists(file.FilePath)) {
                if (file.Parent != null) {
                    await file.Parent.RefreshAsync();
                }

                return;
            }

            ClassViewModel existing = this.ClassManager.Classes.FirstOrDefault(x => x.FilePath == file.FilePath);
            if (existing != null) {
                this.ClassManager.ActiveClass = existing;
                return;
            }

            await this.ClassManager.OpenClassFromFile(file.FilePath);
        }

        private async Task ExplorerOnOpenZipFile(ZipFileEntryViewModel file) {
            ZipArchiveEntry entry = file.Entry;
            if (entry == null) {
                return;
            }

            string name = entry.FullName;
            ClassViewModel existing = this.ClassManager.Classes.FirstOrDefault(x => x.Node.Name?.Name == name);
            if (existing != null) {
                this.ClassManager.ActiveClass = existing;
                return;
            }

            Stream stream;
            try {
                stream = entry.Open();
            }
            catch (Exception e) {
                await IoC.MessageDialogs.ShowMessageExAsync("Open File Failed", "Failed to open file stream at " + file.FullZipPath, e.GetToString());
                return;
            }

            try {
                await this.ClassManager.OpenClassFromStreamAsync(stream);
            }
            finally {
                stream.Close();
            }
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