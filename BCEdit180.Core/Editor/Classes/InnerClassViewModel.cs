using JavaAsm;
using JavaAsm.CustomAttributes;

namespace BCEdit180.Core.Editor.Classes {
    public class InnerClassViewModel : BaseViewModel {
        public ClassAttributeEditorViewModel Class { get; }

        private string innerClassName;
        public string InnerClassName {
            get => this.innerClassName;
            set => this.RaisePropertyChanged(ref this.innerClassName, value);
        }

        private string outerClassName;
        public string OuterClassName {
            get => this.outerClassName;
            set => this.RaisePropertyChanged(ref this.outerClassName, value);
        }

        private string customInnerName;
        private string innerName;
        public string InnerName {
            get => this.innerName;
            set => this.RaisePropertyChanged(ref this.innerName, value);
        }

        private ClassAccessModifiers classAccess;
        public ClassAccessModifiers ClassAccess {
            get => this.classAccess;
            set => this.RaisePropertyChanged(ref this.classAccess, value);
        }

        public InnerClassViewModel(ClassAttributeEditorViewModel clazz) {
            this.Class = clazz;
        }

        public InnerClassViewModel(ClassAttributeEditorViewModel clazz, InnerClass inner) : this(clazz) {
            this.Load(inner);
        }

        public void Load(InnerClass node) {
            this.OuterClassName = node.OuterClassName?.Name ?? ""; // can be null in some cases cases
            this.InnerClassName = node.InnerClassName?.Name ?? "";
            this.InnerName = node.InnerName;
            if (this.InnerName == null && this.InnerClassName != null) {
                this.InnerName = ClassName.GetSimpleName(this.InnerClassName);
                this.customInnerName = this.InnerName;
            }
            else {
                this.customInnerName = null;
            }

            this.ClassAccess = node.Access;
        }

        public void Save(InnerClass node) {
            // default to null inner/outer class
            node.InnerClassName = string.IsNullOrWhiteSpace(this.InnerClassName) ? null : new ClassName(this.InnerClassName);
            node.OuterClassName = string.IsNullOrWhiteSpace(this.OuterClassName) ? null : new ClassName(this.OuterClassName);
            if (this.customInnerName != null && this.customInnerName == this.InnerName) {
                node.InnerName = null;
            }
            else {
                node.InnerName = string.IsNullOrEmpty(this.InnerName) ? null : this.InnerName;
            }

            node.Access = this.ClassAccess;
        }
    }
}