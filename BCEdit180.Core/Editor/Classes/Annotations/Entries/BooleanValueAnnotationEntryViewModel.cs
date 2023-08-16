using BCEdit180.Core.Utils;
using JavaAsm.CustomAttributes.Annotation;

namespace BCEdit180.Core.Editor.Classes.Annotations.Entries {
    public class BooleanValueAnnotationEntryViewModel : BaseAnnotationEntryViewModel {
        private bool state;
        public bool State {
            get => this.state;
            set {
                this.RaisePropertyChanged(ref this.state, value);
                this.value.ConstValue = this.value?.ConstValue ?? BoolBox.False;
            }
        }

        public BooleanValueAnnotationEntryViewModel(AnnotationViewModel annotation, AnnotationNode.ElementValuePair entry) : base(annotation, entry) {

        }

        public override void Load(AnnotationNode.ElementValuePair entry) {
            base.Load(entry);
            this.State = bool.TryParse(entry.Value.ConstValue?.ToString() ?? "False", out bool bVal) && bVal;
        }

        public override void Save(AnnotationNode.ElementValuePair entry) {
            base.Save(entry);
            entry.Value.ConstValue = this.State.Box();
        }
    }
}