using System.Collections.ObjectModel;
using JavaAsm;

namespace BCEdit180.Core.Editor.Classes.Fields {
    public class FieldListViewModel : BaseViewModel {
        public ObservableCollection<FieldViewModel> Fields { get; }

        private FieldViewModel selectedField;

        public FieldViewModel SelectedField {
            get => this.selectedField;
            set => this.RaisePropertyChanged(ref this.selectedField, value);
        }

        private int previousIndex;
        private int selectedIndex;

        public int SelectedIndex {
            get => this.selectedIndex;
            set {
                this.previousIndex = this.selectedIndex;
                this.RaisePropertyChanged(ref this.selectedIndex, value);
            }
        }

        public ClassViewModel Class { get; }

        public FieldListViewModel(ClassViewModel clazz) {
            this.Class = clazz;
            this.Fields = new ObservableCollection<FieldViewModel>();
        }

        public void Load(ClassNode node) {
            this.Fields.Clear();
            foreach (FieldNode field in node.Fields) {
                this.Fields.Add(new FieldViewModel(this, field));
            }

            if (this.previousIndex >= 0 && this.previousIndex < this.Fields.Count) {
                this.SelectedIndex = this.previousIndex;
            }
        }

        public void Save(ClassNode node) {
            foreach (FieldViewModel field in this.Fields) {
                field.Save(node);
            }
        }
    }
}