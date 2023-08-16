using System.Threading.Tasks;
using BCEdit180.Core;
using BCEdit180.Core.Editor.Classes.Editors;
using BCEdit180.Utils;
using JavaAsm;

namespace BCEdit180.Editor.Controls.Access {
    [ServiceImplementation(typeof(IAccessEditorService))]
    public class AccessEditorService : IAccessEditorService {
        public Task<MethodAccessModifiers?> EditMethodAccessAsync(MethodAccessModifiers? template) {
            return DispatcherUtils.InvokeAsync(() => this.EditMethodAccess(template));
        }

        public Task<FieldAccessModifiers?> EditFieldAccessAsync(FieldAccessModifiers? template) {
            return DispatcherUtils.InvokeAsync(() => this.EditFieldAccess(template));
        }

        public Task<ClassAccessModifiers?> EditClassAccessAsync(ClassAccessModifiers? template) {
            return DispatcherUtils.InvokeAsync(() => this.EditClassAccess(template));
        }

        public MethodAccessModifiers? EditMethodAccess(MethodAccessModifiers? template) {
            MethodAccessEditorWindow window = new MethodAccessEditorWindow();
            window.DataContext = new MethodAccessEditorViewModel(window);
            ((MethodAccessEditorViewModel) window.DataContext).Modifiers = template ?? (MethodAccessModifiers.Public | MethodAccessModifiers.Static);
            return window.ShowDialog() == true ? (MethodAccessModifiers?) ((MethodAccessEditorViewModel) window.DataContext).Modifiers : null;
        }

        public FieldAccessModifiers? EditFieldAccess(FieldAccessModifiers? template) {
            FieldAccessEditorWindow window = new FieldAccessEditorWindow();
            window.DataContext = new FieldAccessEditorViewModel(window);
            ((FieldAccessEditorViewModel) window.DataContext).Modifiers = template ?? (FieldAccessModifiers.Public | FieldAccessModifiers.Static);
            return window.ShowDialog() == true ? (FieldAccessModifiers?) ((FieldAccessEditorViewModel) window.DataContext).Modifiers : null;
        }

        public ClassAccessModifiers? EditClassAccess(ClassAccessModifiers? template) {
            ClassAccessEditorWindow window = new ClassAccessEditorWindow();
            window.DataContext = new ClassAccessEditorViewModel(window);
            ((ClassAccessEditorViewModel) window.DataContext).Modifiers = template ?? (ClassAccessModifiers.Public | ClassAccessModifiers.Static);
            return window.ShowDialog() == true ? (ClassAccessModifiers?) ((ClassAccessEditorViewModel) window.DataContext).Modifiers : null;
        }
    }
}