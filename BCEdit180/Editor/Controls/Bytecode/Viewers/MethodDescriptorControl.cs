using System.Windows.Controls;
using BCEdit180.Core.Editor.Classes.Utils;

namespace BCEdit180.Editor.Controls.Bytecode.Viewers {
    public class MethodDescriptorControl : Control {
        public MethodDescriptorViewModel MethodDescriptor {
            get => (MethodDescriptorViewModel) this.DataContext;
            set => this.DataContext = value;
        }
    }
}
