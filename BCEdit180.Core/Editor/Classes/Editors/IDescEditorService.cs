using System.Threading.Tasks;
using JavaAsm;

namespace BCEdit180.Core.Editor.Classes.Editors {
    public interface IDescEditorService {
        Task<TypeDescriptor> EditTypeDesc(bool allowPrimitive, bool allowObject, int defShow, TypeDescriptor def);
        Task<MethodDescriptor> EditMethodDesc(MethodDescriptor def);
    }
}