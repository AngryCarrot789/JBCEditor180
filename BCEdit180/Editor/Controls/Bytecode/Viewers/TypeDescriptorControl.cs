using System.Windows.Controls;
using BCEdit180.Core.Editor.Classes.Descriptors;

namespace BCEdit180.Editor.Controls.Bytecode.Viewers {
    public class TypeDescriptorControl : Control {
        public TypeDescViewModel TypeDesc {
            get => (TypeDescViewModel) this.DataContext;
            set => this.DataContext = value;
        }
    }
}
