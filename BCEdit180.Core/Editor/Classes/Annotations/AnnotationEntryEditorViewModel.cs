namespace BCEdit180.Core.Editor.Classes.Annotations {
    public class AnnotationEntryEditorViewModel : BaseViewModel {
        private ElementValueTagXAML selectedPrimitive;
        public ElementValueTagXAML SelectedPrimitive {
            get => this.selectedPrimitive;
            set => this.RaisePropertyChanged(ref this.selectedPrimitive, value);
        }
    }
}