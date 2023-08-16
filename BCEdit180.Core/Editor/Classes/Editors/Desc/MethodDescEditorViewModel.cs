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

        public ObservableCollection<TypeDescViewModel> Parameters { get; }

        public ObservableCollection<TypeDescViewModel> SelectedParameters { get; }

        /// <summary>
        /// Gets or sets the descriptor. Setting this property will modify <see cref="ReturnType"/> and <see cref="Parameters"/> in this instance
        /// </summary>
        public MethodDescriptor Descriptor {
            get => new MethodDescriptor(this.ReturnType ?? new TypeDescriptor(PrimitiveType.Void, 0), this.Parameters.Select(a => a.TypeDescriptor).ToList());
            set {
                this.Parameters.Clear();
                if (value == null) {
                    this.ReturnType = new TypeDescriptor(PrimitiveType.Void, 0);
                }
                else {
                    this.ReturnType = value.ReturnType;
                    this.Parameters.AddAll(value.ArgumentTypes.Select(x => new TypeDescViewModel(x)));
                }
            }
        }

        public ICommand AddNewParameterCommand { get; }

        public ICommand RemoveSelectedCommand { get; }

        public ICommand EditReturnTypeCommand { get; }

        public MethodDescEditorViewModel(IDialog dialog) : base(dialog) {
            this.Parameters = new ObservableCollection<TypeDescViewModel>();
            this.SelectedParameters = new ObservableCollection<TypeDescViewModel>();
            this.ReturnType = new TypeDescriptor(PrimitiveType.Void, 0);
            this.AddNewParameterCommand = new AsyncRelayCommand(this.AddNewParameter);
            this.RemoveSelectedCommand = new RelayCommand(this.RemoveSelectedAction);
            this.EditReturnTypeCommand = new AsyncRelayCommand(this.EditReturnType);
        }

        public MethodDescEditorViewModel(IDialog dialog, MethodDescriptor desc) : this(dialog) {
            this.Descriptor = desc;
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
            foreach (TypeDescViewModel desc in this.SelectedParameters) {
                this.Parameters.Remove(desc);
            }
        }
    }
}