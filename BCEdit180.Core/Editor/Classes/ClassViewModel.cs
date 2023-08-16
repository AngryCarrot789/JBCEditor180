using BCEdit180.Core.Editor.Classes.Fields;
using BCEdit180.Core.Editor.Classes.Methods;
using JavaAsm;

namespace BCEdit180.Core.Editor.Classes {
    /// <summary>
    /// Stores all information about a java class file
    /// </summary>
    public class ClassViewModel : BaseViewModel {
        public ClassManagerViewModel ClassManager { get; set; }

        public ClassInfoViewModel ClassInfo { get; }

        public ClassAttributeEditorViewModel ClassAttributes { get; }

        public MethodListViewModel MethodList { get; }

        public FieldListViewModel FieldList { get; }

        public SourceCodeViewModel SourceCode { get; }

        public ClassNode Node { get; set; }

        public ClassViewModel() {
            this.ClassInfo = new ClassInfoViewModel(this);
            this.MethodList = new MethodListViewModel(this);
            this.FieldList = new FieldListViewModel(this);
            this.SourceCode = new SourceCodeViewModel(this);
            this.ClassAttributes = new ClassAttributeEditorViewModel(this);
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
    }
}