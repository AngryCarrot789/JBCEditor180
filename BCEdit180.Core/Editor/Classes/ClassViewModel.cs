using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using BCEdit180.Core.AdvancedContextService;
using BCEdit180.Core.Editor.Classes.Fields;
using BCEdit180.Core.Editor.Classes.Methods;
using BCEdit180.Core.Utils;
using BCEdit180.Core.Views.Dialogs.Message;
using JavaAsm;
using JavaAsm.IO;

namespace BCEdit180.Core.Editor.Classes {
    /// <summary>
    /// Stores all information about a java class file
    /// </summary>
    public class ClassViewModel : BaseViewModel {
        private static readonly MessageDialog ReplaceFileDialog;

        static ClassViewModel() {
            ReplaceFileDialog = Dialogs.OkCancelDialog.Clone();
            ReplaceFileDialog.ShowAlwaysUseNextResultOption = true;
        }

        public ClassManagerViewModel ClassManager { get; set; }

        public ClassInfoViewModel ClassInfo { get; }

        public ClassAttributeEditorViewModel ClassAttributes { get; }

        public MethodListViewModel MethodList { get; }

        public FieldListViewModel FieldList { get; }

        public SourceCodeViewModel SourceCode { get; }

        public ClassNode Node { get; set; }

        private string filePath;

        public string FilePath {
            get => this.filePath;
            private set {
                this.RaisePropertyChanged(ref this.filePath, value);
                this.SaveCommand.RaiseCanExecuteChanged();
                this.SaveAsCommand.RaiseCanExecuteChanged();
                this.ReloadCommand.RaiseCanExecuteChanged();
            }
        }

        public AsyncRelayCommand SaveCommand { get; }

        public AsyncRelayCommand SaveAsCommand { get; }

        public AsyncRelayCommand ReloadCommand { get; }

        public ClassViewModel() {
            this.ClassInfo = new ClassInfoViewModel(this);
            this.MethodList = new MethodListViewModel(this);
            this.FieldList = new FieldListViewModel(this);
            this.SourceCode = new SourceCodeViewModel(this);
            this.ClassAttributes = new ClassAttributeEditorViewModel(this);

            this.SaveCommand = new AsyncRelayCommand(this.SaveActionAsync);
            this.SaveAsCommand = new AsyncRelayCommand(this.SaveAsActionAsync);
            this.ReloadCommand = new AsyncRelayCommand(this.ReloadFileAction, () => !string.IsNullOrEmpty(this.FilePath));
        }

        public async Task<bool> ReloadFileAction() {
            if (string.IsNullOrEmpty(this.FilePath) || !File.Exists(this.FilePath)) {
                return false;
            }

            return await this.LoadClassFromFile(this.FilePath);
        }

        public async Task<bool> LoadClassFromFile(string file) {
#if DEBUG
            using (BufferedStream input = new BufferedStream(File.OpenRead(file))) {
                this.Node = await ClassFile.ParseClassAsync(input);
            }
#else
            try {
                using (BufferedStream input = new BufferedStream(File.OpenRead(file))) {
                    this.Node = await ClassFile.ParseClassAsync(input);
                }
            }
            catch (Exception e) {
                await IoC.MessageDialogs.ShowMessageExAsync("Load Error", "Failed to read class from file", e.GetToString());
                return false;
            }
#endif

#if DEBUG
            this.Load(this.Node);
#else
            try {
                this.Load(this.Node);
            }
            catch (Exception e) {
                await IoC.MessageDialogs.ShowMessageExAsync("Load Error", "Failed to load class object into UI", e.GetToString());
                return false;
            }
#endif

            return true;
        }

        public async Task SaveActionAsync() {
            if (string.IsNullOrEmpty(this.FilePath)) {
                await this.SaveAsActionAsync();
            }
            else {
                try {
                    if (File.Exists(this.FilePath) && !File.Exists(this.FilePath + "_backup")) {
                        File.Move(this.FilePath, this.FilePath + "_backup");
                    }
                }
                catch (Exception e) {
                    Debug.WriteLine("Exception while creating backup file : " + e);
                }

                await this.SaveToFile(this.FilePath);
            }
        }

        public async Task<bool> SaveAsActionAsync() {
            string path = await IoC.FilePicker.SaveFile(Filters.ClassAndAll);
            if (string.IsNullOrEmpty(path)) {
                return false;
            }

            this.FilePath = path;
            return await this.SaveToFile(path);
        }

        public async Task<bool> SaveToFile(string file) {
#if DEBUG
            this.Save(this.Node);
#else
            try {
                this.Save(this.Node);
            }
            catch (Exception e) {
                await IoC.MessageDialogs.ShowMessageExAsync("Save error", "Failed to save class object from UI", e.GetToString());
                return false;
            }
#endif

#if DEBUG
            using (BufferedStream output = new BufferedStream(File.OpenWrite(file))) {
                await ClassFile.WriteClassAsync(output, this.Node);
            }
#else
            try {
                using (BufferedStream output = new BufferedStream(File.OpenWrite(file))) {
                    await ClassFile.WriteClassAsync(output, this.Node);
                }
            }
            catch (Exception e) {
                await IoC.MessageDialogs.ShowMessageExAsync("Save error", "Failed to write class to file", e.GetToString());
                return false;
            }
#endif

            return true;
        }

        public ClassViewModel(ClassNode node) : this() {
            this.Load(node);
        }

        public void Load(ClassNode node) {
            this.Node = node;
            this.ClassInfo.Load(node);
            this.ClassAttributes.Load(node);
            this.MethodList.Load(node);
            this.FieldList.Load(node);
        }

        public void Save(ClassNode node) {
            this.Node = node;
            node.Attributes.Clear();
            this.ClassInfo.Save(node);
            this.ClassAttributes.Save(node);
            this.MethodList.Save(node);
            this.FieldList.Save(node);
        }

        public ClassNode Save() {
            if (this.Node == null) {
                this.Node = new ClassNode();
            }

            this.Save(this.Node);
            return this.Node;
        }

        public void SetFilePath(string path) {
            this.FilePath = path;
        }
    }
}