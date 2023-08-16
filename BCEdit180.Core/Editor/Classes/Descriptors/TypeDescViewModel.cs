using System.Windows.Input;
using JavaAsm;

namespace BCEdit180.Core.Editor.Classes.Descriptors {
    public class TypeDescViewModel : BaseViewModel {
        private TypeDescriptor typeDescriptor;
        public TypeDescriptor TypeDescriptor {
            get => this.typeDescriptor;
            set => this.RaisePropertyChanged(ref this.typeDescriptor, value);
        }

        public ICommand EditFieldDescriptorCommand { get; }

        public TypeDescViewModel() : this(new TypeDescriptor(PrimitiveType.Integer, 0)) {

        }

        public TypeDescViewModel(TypeDescriptor typeDescriptor) {
            this.EditFieldDescriptorCommand = new RelayCommand(this.EditTypeDescriptorAction);
            this.TypeDescriptor = typeDescriptor;
        }

        public void EditTypeDescriptorAction() {

        }
    }
}