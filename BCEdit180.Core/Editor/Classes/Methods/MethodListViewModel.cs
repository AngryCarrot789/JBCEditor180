using System.Collections.ObjectModel;
using System.Threading.Tasks;
using JavaAsm;

namespace BCEdit180.Core.Editor.Classes.Methods {
    public class MethodListViewModel : BaseViewModel {
        public ObservableCollection<MethodViewModel> Methods { get; }

        private MethodViewModel selectedMethod;
        public MethodViewModel SelectedMethod {
            get => this.selectedMethod;
            set => this.RaisePropertyChanged(ref this.selectedMethod, value);
        }

        private int lastSaveIndex;
        private int selectedIndex;
        public int SelectedIndex {
            get => this.selectedIndex;
            set => this.RaisePropertyChanged(ref this.selectedIndex, value);
        }

        public ClassViewModel Class { get; }

        public RelayCommand<MethodViewModel> EditMethodAccessCommand { get; }

        public RelayCommand<MethodViewModel> EditMethodDescriptorCommand { get; }

        public MethodListViewModel(ClassViewModel klass) {
            this.Class = klass;
            this.Methods = new ObservableCollection<MethodViewModel>();
            this.EditMethodAccessCommand = new RelayCommand<MethodViewModel>(async (x) => {
                MethodAccessModifiers? result = await IoC.AccessEditors.EditMethodAccessAsync(x.Access);
                if (result.HasValue)
                    x.Access = result.Value;
            });

            this.EditMethodDescriptorCommand = new RelayCommand<MethodViewModel>(async (x) => {
                MethodDescriptor result = await IoC.TypeDescEditors.EditMethodDesc(x.MethodDescriptor);
                if (result != null)
                    x.MethodDescriptor = result;
            });
        }

        public void Load(ClassNode node) {
            this.Methods.Clear();
            foreach (MethodNode method in node.Methods) {
                this.Methods.Add(new MethodViewModel(method) {
                    MethodList = this
                });
            }

            if (this.lastSaveIndex >= 0 && this.lastSaveIndex < this.Methods.Count) {
                this.SelectedIndex = this.lastSaveIndex;
            }

            this.lastSaveIndex = 0;
        }

        public void Save(ClassNode node) {
            this.lastSaveIndex = this.SelectedIndex;
            foreach (MethodViewModel method in this.Methods) {
                method.Save(node);
            }
        }
    }
}