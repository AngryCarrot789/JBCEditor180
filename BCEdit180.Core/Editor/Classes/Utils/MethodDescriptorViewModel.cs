using System.Windows.Input;
using BCEdit180.Core.Editor.Classes.Methods;
using BCEdit180.Core.Views.Dialogs;
using JavaAsm;

namespace BCEdit180.Core.Editor.Classes.Utils {
    /// <summary>
    /// A view model that encapsulated a method descriptor
    /// </summary>
    public class MethodDescriptorViewModel : BaseViewModel, IMethodDescriptable {
        private MethodDescriptor methodDescriptor;
        public MethodDescriptor MethodDescriptor {
            get => this.methodDescriptor;
            set => this.RaisePropertyChanged(ref this.methodDescriptor, value);
        }

        public MethodDescriptorViewModel() {
        }
    }
}