using JavaAsm;

namespace BCEdit180.Core.Editor.Classes {
    public class SourceDebugExtensionViewModel : BaseViewModel {
        private string value;
        public string Value {
            get => this.value;
            set => this.RaisePropertyChanged(ref this.value, value);
        }

        public void Load(ClassNode node) {
            this.Value = node.SourceDebugExtension;
        }

        public void Save(ClassNode node) {
            node.SourceDebugExtension = string.IsNullOrWhiteSpace(this.Value) ? null : this.Value;
        }
    }
}