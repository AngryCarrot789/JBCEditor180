using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BCEdit180.Core.Views.Dialogs.UserInputs;
using JavaAsm;

namespace BCEdit180.Core.Editor.Classes {
    public class ClassInfoViewModel : BaseViewModel {
        public ClassViewModel Class { get; }

        private int minorVersion;
        public int MinorVersion {
            get => this.minorVersion;
            set => this.RaisePropertyChanged(ref this.minorVersion, value);
        }

        private ClassVersion majorVersion;
        public ClassVersion MajorVersion {
            get => this.majorVersion;
            set => this.RaisePropertyChanged(ref this.majorVersion, value);
        }

        private ClassAccessModifiers accessFlags;
        public ClassAccessModifiers AccessFlags {
            get => this.accessFlags;
            set => this.RaisePropertyChanged(ref this.accessFlags, value);
        }

        private string fullClassName;

        /// <summary>
        /// Gets or sets the full class name, which is the packages and class name combined with '/' characters
        /// </summary>
        public string FullClassName {
            get => this.fullClassName;
            set {
                this.RaisePropertyChanged(ref this.fullClassName, value);
                this.RaisePropertyChanged(nameof(this.SimpleClassName));
            }
        }

        /// <summary>
        /// Returns only the actual class name itself (e.g. Main), without the packages
        /// </summary>
        public string SimpleClassName {
            get {
                if (this.fullClassName == null)
                    return null;
                int index = this.fullClassName.LastIndexOf('/');
                return index == -1 ? this.fullClassName : this.fullClassName.Substring(index + 1);
            }
        }

        private string superName;
        public string SuperName {
            get => this.superName;
            set => this.RaisePropertyChanged(ref this.superName, value);
        }

        private string signature;
        public string Signature {
            get => this.signature;
            set => this.RaisePropertyChanged(ref this.signature, value);
        }

        private string sourceDebugInfo;
        public string SourceDebugInfo {
            get => this.sourceDebugInfo;
            set => this.RaisePropertyChanged(ref this.sourceDebugInfo, value);
        }

        public ObservableCollection<ClassNameViewModel> Interfaces { get; }
        public RelayCommand<ClassNameViewModel> RemoveInterfaceCommand { get; }
        public AsyncRelayCommand<ClassNameViewModel> RenameInterfaceCommand { get; }
        public AsyncRelayCommand AddNewInterfaceCommand { get; set; }

        public AsyncRelayCommand EditAccessCommand { get; }

        public readonly InputValidator InterfaceNameValidator;

        public ClassInfoViewModel(ClassViewModel klass) {
            this.Class = klass;
            this.Interfaces = new ObservableCollection<ClassNameViewModel>();

            this.InterfaceNameValidator = InputValidator.FromFunc((x) => {
                if (x == null) {
                    return null;
                }

                string itf = x.Replace('.', '/');
                return this.Interfaces.Any(y => y.FullName == itf) ? "Interface already added with that name" : null;
            });

            this.RemoveInterfaceCommand = new RelayCommand<ClassNameViewModel>((c) => {
                this.Interfaces.Remove(c);
            });

            this.RenameInterfaceCommand = new AsyncRelayCommand<ClassNameViewModel>(async (x) => {
                string name = await IoC.UserInput.ShowSingleInputDialogAsync("Change interface", "Input a new interface name", x.FullName, this.InterfaceNameValidator);
                if (!string.IsNullOrEmpty(name)) {
                    name = name.Replace('.', '/');
                    if (this.Interfaces.All(y => y.FullName != name)) {
                        x.FullName = name;
                    }
                }
            });

            this.AddNewInterfaceCommand = new AsyncRelayCommand(async () => {
                string name = await IoC.UserInput.ShowSingleInputDialogAsync("Add interface", "Input a new interface name", "my/interface/Here", this.InterfaceNameValidator);
                if (!string.IsNullOrEmpty(name)) {
                    name = name.Replace('.', '/');
                    if (this.Interfaces.All(y => y.FullName != name)) {
                        this.Interfaces.Add(new ClassNameViewModel(name));
                    }
                }
            });

            this.EditAccessCommand = new AsyncRelayCommand(async () => {
                ClassAccessModifiers? result = await IoC.AccessEditors.EditClassAccessAsync(this.AccessFlags);
                if (result.HasValue) {
                    this.AccessFlags = result.Value;
                }
            });
        }

        public void Load(ClassNode node) {
            this.MinorVersion = node.MinorVersion;
            this.MajorVersion = node.MajorVersion;
            this.AccessFlags = node.Access;
            this.FullClassName = node.Name.Name;
            this.SuperName = node.SuperName.Name;
            this.Signature = node.Signature;
            this.SourceDebugInfo = node.SourceDebugExtension ?? "";
            this.Interfaces.Clear();
            foreach (ClassName name in node.Interfaces) {
                this.Interfaces.Add(new ClassNameViewModel(name));
            }
        }

        public void Save(ClassNode node) {
            node.MinorVersion = (ushort) this.MinorVersion;
            node.MajorVersion = this.MajorVersion;
            node.Access = this.AccessFlags;
            node.Name = new ClassName(this.FullClassName);
            node.SuperName = new ClassName(this.SuperName);
            node.Signature = this.Signature;
            node.SourceDebugExtension = string.IsNullOrWhiteSpace(this.SourceDebugInfo) ? null : this.SourceDebugInfo;
            node.Interfaces = new List<ClassName>(this.Interfaces.Select(c => new ClassName(c.FullName)));
        }
    }
}