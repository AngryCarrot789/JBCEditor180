using System.Collections.ObjectModel;
using System.Linq;
using JavaAsm;

namespace BCEdit180.Core.Editor.Classes.Fields {
    public class FieldListViewModel : BaseViewModel {
        public ObservableCollection<FieldViewModel> Fields { get; }

        private FieldViewModel primarySelectedField;

        public FieldViewModel PrimarySelectedField {
            get => this.primarySelectedField;
            set => this.RaisePropertyChanged(ref this.primarySelectedField, value);
        }

        public ObservableCollection<FieldViewModel> SelectedFields { get; }

        private int previousIndex;
        private int primarySelectedIndex;
        public int PrimarySelectedIndex {
            get => this.primarySelectedIndex;
            set {
                this.previousIndex = this.primarySelectedIndex;
                this.RaisePropertyChanged(ref this.primarySelectedIndex, value);
            }
        }

        public ClassViewModel Class { get; }

        public RelayCommand DeleteSelectedFieldsCommand { get; }

        public FieldListViewModel(ClassViewModel clazz) {
            this.Class = clazz;
            this.Fields = new ObservableCollection<FieldViewModel>();
            this.SelectedFields = new ObservableCollection<FieldViewModel>();
            this.DeleteSelectedFieldsCommand = new RelayCommand(() => {
                foreach (FieldViewModel field in this.SelectedFields.ToList()) {
                    this.Fields.Remove(field);
                }
            });
        }

        public void Load(ClassNode node) {
            this.Fields.Clear();
            foreach (FieldNode field in node.Fields) {
                this.Fields.Add(new FieldViewModel(this, field));
            }

            if (this.previousIndex >= 0 && this.previousIndex < this.Fields.Count) {
                this.PrimarySelectedIndex = this.previousIndex;
            }
        }

        public void Save(ClassNode node) {
            foreach (FieldViewModel field in this.Fields) {
                field.Save(node);
            }
        }
    }
}