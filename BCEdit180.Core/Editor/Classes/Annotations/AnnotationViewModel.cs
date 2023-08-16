using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BCEdit180.Core.Editor.Classes.Annotations.Entries;
using BCEdit180.Core.Editor.Classes.Descriptors;
using JavaAsm;
using JavaAsm.CustomAttributes.Annotation;

namespace BCEdit180.Core.Editor.Classes.Annotations {
    public class AnnotationViewModel : BaseViewModel {
        private readonly AnnotationNode node;

        public AnnotationNode Node { get => this.node; }

        private TypeDescViewModel type;
        public TypeDescViewModel Type {
            get => this.type;
            set => this.RaisePropertyChanged(ref this.type, value);
        }

        public ObservableCollection<BaseAnnotationEntryViewModel> Entries { get; }

        public AnnotationViewModel() {
            this.Entries = new ObservableCollection<BaseAnnotationEntryViewModel>();
        }

        public AnnotationViewModel(AnnotationNode node) : this() {
            this.node = node;
            this.Load(node);
        }

        // TODO: Load and save annotations

        public void Load(AnnotationNode node) {
            if (node.Type != null) {
                this.Type = new TypeDescViewModel(node.Type);
            }

            this.Entries.Clear();
            foreach (BaseAnnotationEntryViewModel item in node.ElementValuePairs.Select(a => BaseAnnotationEntryViewModel.Of(this, a))) {
                if (item != null) {
                    this.Entries.Add(item);
                }
            }
        }

        public void Save(AnnotationNode node) {
            node.Type = this.Type?.TypeDescriptor;
            node.ElementValuePairs = new List<AnnotationNode.ElementValuePair>(this.Entries.Select(a => a.SaveAndGet()));
        }
    }
}
