using System.Threading.Tasks;
using BCEdit180.Core;
using BCEdit180.Core.Editor.Classes.Editors;
using BCEdit180.Core.Editor.Classes.Editors.Desc;
using BCEdit180.Utils;
using JavaAsm;

namespace BCEdit180.Editor.Controls.Descs {
    [ServiceImplementation(typeof(IDescEditorService))]
    public class DescEditorService : IDescEditorService {
        public Task<TypeDescriptor> EditTypeDesc(bool allowPrimitive, bool allowObject, int defShow, TypeDescriptor def) {
            return DispatcherUtils.InvokeAsync(() => ShowTypeDescDialog(allowPrimitive, allowObject, defShow, def));
        }

        public Task<MethodDescriptor> EditMethodDesc(MethodDescriptor def) {
            return DispatcherUtils.InvokeAsync(() => ShowMethodDescDialog(def));
        }

        public static TypeDescriptor ShowTypeDescDialog(bool primitives, bool objects, int defShow = 0, TypeDescriptor def = null) {
            TypeDescEditorWindow dialog = new TypeDescEditorWindow();
            TypeDescEditorViewModel editor = new TypeDescEditorViewModel(dialog) {
                AllowPrimitive = primitives,
                AllowClass = objects
            };

            switch (defShow) {
                case 0: editor.IsObject = true; break;
                case 1: editor.IsPrimitive = true; break;
            }

            if (def != null) {
                editor.SetTypeDescriptor(def);
            }

            dialog.DataContext = editor;
            if (dialog.ShowDialog() != true) {
                return null;
            }

            TypeDescriptor desc;
            if (editor.IsObject) {
                if (string.IsNullOrEmpty(editor.PreviewClassName) || !ClassName.TryParse(editor.PreviewClassName, out ClassName name)) {
                    return null;
                }

                desc = new TypeDescriptor(name, editor.ArrayDepth);
            }
            else {
                desc = new TypeDescriptor(editor.SelectedPrimitive, editor.ArrayDepth);
            }

            return desc;
        }

        public static MethodDescriptor ShowMethodDescDialog(MethodDescriptor def) {
            MethodDescEditorWindow dialog = new MethodDescEditorWindow();
            MethodDescEditorViewModel editor = new MethodDescEditorViewModel(dialog);
            if (def != null) {
                editor.Descriptor = def;
            }

            dialog.DataContext = editor;
            if (dialog.ShowDialog() != true) {
                return null;
            }

            return editor.Descriptor;
        }
    }
}