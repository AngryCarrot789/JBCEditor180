using System.Collections.Generic;
using System.Collections.ObjectModel;
using JavaAsm;
using JavaAsm.CustomAttributes;

namespace BCEdit180.Core.Editor.Classes {
    public class ClassAttributeEditorViewModel : BaseViewModel {
        public ClassViewModel Class { get; }

        public SourceFileViewModel SourceFile { get; }

        public SourceDebugExtensionViewModel SourceDebug { get; }

        public ObservableCollection<InnerClassViewModel> InnerClasses { get; }

        public ObservableCollection<InnerClassViewModel> SelectedInnerClasses { get; }

        public EnclosingMethodViewModel EnclosingMethod { get; }

        private bool isEnabledSourceFile;
        public bool IsEnabledSourceFile {
            get => this.isEnabledSourceFile;
            set => this.RaisePropertyChanged(ref this.isEnabledSourceFile, value);
        }

        private bool isEnabledSourceDebug;
        public bool IsEnabledSourceDebug {
            get => this.isEnabledSourceDebug;
            set => this.RaisePropertyChanged(ref this.isEnabledSourceDebug, value);
        }

        private bool isEnabledInnerClasses;
        public bool IsEnabledInnerClasses {
            get => this.isEnabledInnerClasses;
            set => this.RaisePropertyChanged(ref this.isEnabledInnerClasses, value);
        }

        private bool isEnabledEnclosingMethod;
        public bool IsEnabledEnclosingMethod {
            get => this.isEnabledEnclosingMethod;
            set => this.RaisePropertyChanged(ref this.isEnabledEnclosingMethod, value);
        }

        public RelayCommand<InnerClassViewModel> RemoveInnerClassCommand { get; }

        public ClassAttributeEditorViewModel(ClassViewModel klass) {
            this.Class = klass;
            this.SourceFile = new SourceFileViewModel();
            this.SourceDebug = new SourceDebugExtensionViewModel();
            this.InnerClasses = new ObservableCollection<InnerClassViewModel>();
            this.SelectedInnerClasses = new ObservableCollection<InnerClassViewModel>();
            this.EnclosingMethod = new EnclosingMethodViewModel();
            this.RemoveInnerClassCommand = new RelayCommand<InnerClassViewModel>((x) => {
                if (x != null) {
                    this.InnerClasses.Remove(x);
                }
            });
        }

        public void Load(ClassNode node) {
            this.InnerClasses.Clear();
            if (string.IsNullOrEmpty(node.SourceFile)) {
                this.IsEnabledSourceFile = false;
                this.SourceFile.SourceFile = null;
            }
            else {
                this.IsEnabledSourceFile = true;
                this.SourceFile.SourceFile = node.SourceFile;
            }

            if (string.IsNullOrEmpty(node.SourceDebugExtension)) {
                this.IsEnabledSourceDebug = false;
                this.SourceDebug.Value = null;
            }
            else {
                this.IsEnabledSourceDebug = true;
                this.SourceDebug.Value = node.SourceDebugExtension;
            }

            if (node.InnerClasses != null && node.InnerClasses.Count > 0) {
                this.IsEnabledInnerClasses = true;
                foreach (InnerClass innerClass in node.InnerClasses) {
                    this.InnerClasses.Add(new InnerClassViewModel(this, innerClass));
                }
            }

            this.EnclosingMethod.Load(node);
            this.IsEnabledEnclosingMethod = node.EnclosingMethod != null;
        }

        public void Save(ClassNode node) {
            node.SourceFile = this.IsEnabledSourceFile ? this.SourceFile.SourceFile : null;
            node.SourceDebugExtension = this.IsEnabledSourceDebug ? this.SourceDebug.Value : null;
            if (this.IsEnabledInnerClasses) {
                List<InnerClass> classes = new List<InnerClass>();
                foreach (InnerClassViewModel clazz in this.InnerClasses) {
                    InnerClass inner = new InnerClass();
                    clazz.Save(inner);
                    classes.Add(inner);
                }

                node.InnerClasses = classes;
            }
            else {
                node.InnerClasses = new List<InnerClass>();
            }

            if (this.IsEnabledEnclosingMethod) {
                this.EnclosingMethod.Save(node);
            }
            else {
                node.EnclosingMethod = null;
            }
        }
    }
}