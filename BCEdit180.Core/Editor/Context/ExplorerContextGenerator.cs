using System.Collections.Generic;
using BCEdit180.Core.Actions.Contexts;
using BCEdit180.Core.AdvancedContextService;
using BCEdit180.Core.Editor.FileSystem;
using BCEdit180.Core.Editor.FileSystem.Physical;
using BCEdit180.Core.Editor.FileSystem.Zip;

namespace BCEdit180.Core.Editor.Context {
    public class ExplorerContextGenerator : IContextGenerator {
        public static ExplorerContextGenerator Instance { get; } = new ExplorerContextGenerator();

        public RelayCommand<string> OpenInExplorerCommand { get; }

        public RelayCommand<string> CopyStringCommand { get; }

        public ExplorerContextGenerator() {
            this.OpenInExplorerCommand = new RelayCommand<string>((x) => {
                if (!string.IsNullOrEmpty(x))
                    IoC.ExplorerService.OpenFileInExplorer(x);
            });

            this.CopyStringCommand = new RelayCommand<string>((x) => {
                if (!string.IsNullOrEmpty(x))
                    IoC.Clipboard.SetText(x);
            });
        }

        public void Generate(List<IContextEntry> list, IDataContext context) {
            if (!context.TryGetContext(out BaseExplorerItemViewModel item)) {
                return;
            }

            FileExplorerViewModel explorer = item.Explorer;
            if (explorer != null || context.TryGetContext(out explorer)) {
                if (item is IOFileItemViewModel || item is ZipFileEntryViewModel)
                    list.Add(new CommandContextEntry("Open", explorer.OpenItemCommand, item));
            }

            if (item is IOFolderItemViewModel)
                list.Add(new CommandContextEntry("Refresh", BaseExplorerItemViewModel.RefreshCommand, item));
            if (item is BaseIOFileItemViewModel) {
                string path = ((BaseIOFileItemViewModel) item).FilePath;
                if (!string.IsNullOrWhiteSpace(path)) {
                    list.Add(new CommandContextEntry("Open in Explorer", this.OpenInExplorerCommand, path));
                    list.Add(new CommandContextEntry("Copy Path", this.CopyStringCommand, path));
                }
            }

            list.Add(SeparatorEntry.Instance);
            list.Add(new CommandContextEntry("Remove", BaseExplorerItemViewModel.RemoveSelfCommand, item));
        }
    }
}