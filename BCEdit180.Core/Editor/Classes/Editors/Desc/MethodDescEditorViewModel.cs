using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using BCEdit180.Core.Editor.Classes.Descriptors;
using BCEdit180.Core.Utils;
using BCEdit180.Core.Views.Dialogs;
using JavaAsm;

namespace BCEdit180.Core.Editor.Classes.Editors.Desc {
    public class MethodDescEditorViewModel : BaseConfirmableDialogViewModel {
        private TypeDescriptor returnType;
        public TypeDescriptor ReturnType {
            get => this.returnType;
            set => this.RaisePropertyChanged(ref this.returnType, value);
        }

        private TypeDescViewModel selectedParameter;
        public TypeDescViewModel SelectedParameter {
            get => this.selectedParameter;
            set => this.RaisePropertyChanged(ref this.selectedParameter, value);
        }

        public MethodDescriptor Descriptor => new MethodDescriptor(this.ReturnType ?? new TypeDescriptor(PrimitiveType.Void, 0), this.Parameters.Select(a => a.TypeDescriptor).ToList());

        public ObservableCollection<TypeDescViewModel> Parameters { get; }

        public ICommand AddNewParameterCommand { get; }

        public ICommand RemoveSelectedCommand { get; }

        public ICommand EditReturnTypeCommand { get; }

        public MethodDescEditorViewModel(IDialog dialog) : base(dialog) {
            this.Parameters = new ObservableCollection<TypeDescViewModel>();
            this.ReturnType = new TypeDescriptor(PrimitiveType.Void, 0);
            this.AddNewParameterCommand = new AsyncRelayCommand(this.AddNewParameter);
            this.RemoveSelectedCommand = new RelayCommand(this.RemoveSelectedAction);
            this.EditReturnTypeCommand = new AsyncRelayCommand(this.EditReturnType);
        }

        public MethodDescEditorViewModel(IDialog dialog, MethodDescriptor desc) : this(dialog) {
            this.Parameters.AddAll(desc.ArgumentTypes.Select(a => new TypeDescViewModel(a)));
            this.ReturnType = desc.ReturnType;
        }

        public async Task EditReturnType() {
            TypeDescriptor result = await IoC.TypeDescEditors.EditTypeDesc(true, true, -1, this.ReturnType);
            if (result != null) {
                this.ReturnType = result;
            }
        }

        public async Task AddNewParameter() {
            TypeDescriptor result = await IoC.TypeDescEditors.EditTypeDesc(true, true, -1, new TypeDescriptor(PrimitiveType.Integer, 0));
            if (result != null) {
                this.Parameters.Add(new TypeDescViewModel(result));
            }
        }

        public void RemoveSelectedAction() {
            if (this.SelectedParameter != null) {
                this.Parameters.Remove(this.SelectedParameter);
                if (this.Parameters.Count > 0) {
                    this.SelectedParameter = this.Parameters[this.Parameters.Count - 1];
                }
            }
        }
    }
}