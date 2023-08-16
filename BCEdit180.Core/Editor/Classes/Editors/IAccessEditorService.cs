using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JavaAsm;

namespace BCEdit180.Editor.Controls.Access {
    public interface IAccessEditorService {
        Task<MethodAccessModifiers?> EditMethodAccessAsync(MethodAccessModifiers? template);
        Task<FieldAccessModifiers?> EditFieldAccessAsync(FieldAccessModifiers? template);
        Task<ClassAccessModifiers?> EditClassAccessAsync(ClassAccessModifiers? template);
    }
}
