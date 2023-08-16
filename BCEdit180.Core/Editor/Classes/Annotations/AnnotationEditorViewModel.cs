using System.Collections.ObjectModel;

namespace BCEdit180.Core.Editor.Classes.Annotations {
    public class AnnotationEditorViewModel : BaseViewModel {
        public ObservableCollection<AnnotationViewModel> Annotations { get; }

        public AnnotationEditorViewModel() {
            this.Annotations = new ObservableCollection<AnnotationViewModel>();
        }
    }
}
