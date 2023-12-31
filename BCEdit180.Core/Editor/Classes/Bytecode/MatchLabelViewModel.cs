using System;
using System.Windows.Input;
using JavaAsm.Instructions.Types;

namespace BCEdit180.Core.Editor.Classes.Bytecode {
    public class MatchLabelViewModel : BaseViewModel {
        private int switchIndex;
        public int SwitchIndex {
            get => this.switchIndex;
            set {
                this.RaisePropertyChanged(ref this.switchIndex, value);
                this.UpdateErrors();
            }
        }

        private long labelIndex;
        public long LabelIndex {
            get => this.labelIndex;
            set => this.RaisePropertyChanged(ref this.labelIndex, value);
        }

        private Label label;
        public Label Label {
            get => this.label;
            set {
                this.label = value;
                this.LabelIndex = value?.Index ?? -1;
            }
        }

        // used to affect the view in some way
        private bool isDefault;
        public bool IsDefault {
            get => this.isDefault;
            set => this.RaisePropertyChanged(ref this.isDefault, value);
        }

        public ICommand SelectJumpDestinationCommand { get; set; }
        public ICommand EditTargetLabelCommand { get; set; }
        public ICommand RemoveSelfCommand { get; set; }

        public Action<MatchLabelViewModel> SelectLabelCallback { get; set; }
        public Action<MatchLabelViewModel> EditTargetLabelCallback { get; set; }
        public Action<MatchLabelViewModel> RemoveSelfCallback { get; set; }

        public MatchLabelViewModel() {
            this.SelectJumpDestinationCommand = new RelayCommand(() => this.SelectLabelCallback?.Invoke(this));
            this.EditTargetLabelCommand = new RelayCommand(() => this.EditTargetLabelCallback?.Invoke(this));
            this.RemoveSelfCommand = new RelayCommand(() => this.RemoveSelfCallback?.Invoke(this));
        }

        private void UpdateErrors() {
            
        }

        public void Load(in int index, in Label target) {
            this.Label = target;
            this.LabelIndex = target.Index;
            this.SwitchIndex = index;
        }

        public override string ToString() {
            return $"SWITCH[{this.SwitchIndex}] -> L{this.LabelIndex}";
        }
    }
}