using System.Threading.Tasks;
using JavaAsm;

namespace BCEdit180.Core.Editor.Classes.Editors {
    public interface IAccessEditorService {
        Task<MethodAccessModifiers?> EditMethodAccessAsync(MethodAccessModifiers? template);
        Task<FieldAccessModifiers?> EditFieldAccessAsync(FieldAccessModifiers? template);
        Task<ClassAccessModifiers?> EditClassAccessAsync(ClassAccessModifiers? template);
    }
}
