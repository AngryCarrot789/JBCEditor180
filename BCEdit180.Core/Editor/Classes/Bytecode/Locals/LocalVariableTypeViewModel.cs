namespace BCEdit180.Core.Editor.Classes.Bytecode.Locals {
    public class LocalVariableTypeViewModel : BaseViewModel {
        private ushort startPc;
        private ushort length;
        private string variableName;
        private string signature;
        private ushort index;

        public ushort StartPC {
            get => this.startPc;
            set => this.RaisePropertyChanged(ref this.startPc, value);
        }

        public ushort Length {
            get => this.length;
            set => this.RaisePropertyChanged(ref this.length, value);
        }

        public string VariableName {
            get => this.variableName;
            set => this.RaisePropertyChanged(ref this.variableName, value);
        }

        public string Signature {
            get => this.signature;
            set => this.RaisePropertyChanged(ref this.signature, value);
        }

        public ushort Index {
            get => this.index;
            set => this.RaisePropertyChanged(ref this.index, value);
        }
    }
}
