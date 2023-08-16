using BCEdit180.Core.PropertyEditing;

namespace BCEdit180.Core.Editor.Classes.PropertyEditors.InnerClasses {
    public class InnerClassPropertyEditorRegistry : PropertyEditorRegistry {
        public static InnerClassPropertyEditorRegistry Instance { get; } = new InnerClassPropertyEditorRegistry();

        public InnerClassPropertyEditorViewModel Editor { get; }

        public InnerClassPropertyEditorRegistry() {
            this.Editor = new InnerClassPropertyEditorViewModel();
            this.Root.AddPropertyEditor("InnerClassEditor", this.Editor);
        }
    }
}