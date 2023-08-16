namespace BCEdit180.Core.Editor.Classes.Bytecode {
    public class OpcodeDescriptorViewModel : BaseViewModel {
        private string header;
        public string Header {
            get => this.header;
            set => this.RaisePropertyChanged(ref this.header, value);
        }

        private string stackTransition;
        public string StackTransition {
            get => this.stackTransition;
            set => this.RaisePropertyChanged(ref this.stackTransition, value);
        }

        private string description;
        public string Description {
            get => this.description;
            set => this.RaisePropertyChanged(ref this.description, value);
        }
    }
}
