using JavaAsm;

namespace BCEdit180.Core.Editor.Classes {
    public class SourceFileViewModel : BaseViewModel {
        private string sourceFile;
        public string SourceFile {
            get => this.sourceFile;
            set => this.RaisePropertyChanged(ref this.sourceFile, value);
        }

        public void Load(ClassNode node) {
            this.SourceFile = node.SourceFile;
        }

        public void Save(ClassNode node) {
            node.SourceFile = string.IsNullOrWhiteSpace(this.SourceFile) ? null : this.SourceFile;
        }
    }
}