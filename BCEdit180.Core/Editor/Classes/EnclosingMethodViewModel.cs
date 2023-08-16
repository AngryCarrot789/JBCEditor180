using JavaAsm;
using JavaAsm.CustomAttributes;

namespace BCEdit180.Core.Editor.Classes {
    public class EnclosingMethodViewModel : BaseViewModel {
        private string className;
        public string ClassName {
            get => this.className;
            set => this.RaisePropertyChanged(ref this.className, value);
        }

        private string methodName;
        public string MethodName {
            get => this.methodName;
            set => this.RaisePropertyChanged(ref this.methodName, value);
        }

        private MethodDescriptor descriptor;
        public MethodDescriptor Descriptor {
            get => this.descriptor;
            set => this.RaisePropertyChanged(ref this.descriptor, value);
        }

        public EnclosingMethodViewModel() {

        }

        public void Load(ClassNode node) {
            if (node.EnclosingMethod != null) {
                this.ClassName = node.EnclosingMethod.Class?.Name;
                this.MethodName = node.EnclosingMethod.MethodName;
                this.Descriptor = node.EnclosingMethod.MethodDescriptor;
            }
            else {
                this.ClassName = null;
                this.MethodName = null;
                this.Descriptor = null;
            }
        }

        public void Save(ClassNode node) {
            if (node.EnclosingMethod == null) {
                node.EnclosingMethod = new EnclosingMethodAttribute();
            }

            node.EnclosingMethod.Class = new ClassName(this.ClassName);
            node.EnclosingMethod.MethodName = this.MethodName;
            node.EnclosingMethod.MethodDescriptor = this.Descriptor;
        }
    }
}