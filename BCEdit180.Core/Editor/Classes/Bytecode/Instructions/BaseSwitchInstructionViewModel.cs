using System.Collections.ObjectModel;

namespace BCEdit180.Core.Editor.Classes.Bytecode.Instructions {
    public abstract class BaseSwitchInstructionViewModel  : BaseInstructionViewModel, IBytecodeEditorAccess {
        private MatchLabelViewModel defaultLabel;
        public MatchLabelViewModel DefaultLabel {
            get => this.defaultLabel;
            set => this.RaisePropertyChanged(ref this.defaultLabel, value);
        }

        private long defaultIndex;
        public long DefaultIndex {
            get => this.defaultIndex;
            set => this.RaisePropertyChanged(ref this.defaultIndex, value);
        }

        public BytecodeEditorViewModel BytecodeEditor { get; set; }

        public ObservableCollection<MatchLabelViewModel> MatchLabels { get; }

        private MatchLabelViewModel selectedLabel;
        public MatchLabelViewModel SelectedLabel {
            get => this.selectedLabel;
            set => this.RaisePropertyChanged(ref this.selectedLabel, value);
        }

        public RelayCommand RemoveSelectedLabelCommand { get; }
        public RelayCommand<MatchLabelViewModel> RemoveSelfCommand { get; }

        protected BaseSwitchInstructionViewModel() {
            this.MatchLabels = new ObservableCollection<MatchLabelViewModel>();
            this.RemoveSelectedLabelCommand = new RelayCommand(this.RemoveSelectedLabelAction);
            this.RemoveSelfCommand = new RelayCommand<MatchLabelViewModel>(this.RemoveSelfAction);
        }

        protected void SetCallbacks(MatchLabelViewModel match) {
            match.SelectLabelCallback = this.SelectLabel;
            match.EditTargetLabelCallback = this.EditLabelTarget;
            match.RemoveSelfCallback = this.RemoveSelfAction;
        }

        private void RemoveSelfAction(MatchLabelViewModel label) {
            this.MatchLabels.Remove(label);
        }

        private void RemoveSelectedLabelAction() {
            if (this.SelectedLabel != null) {
                this.MatchLabels.Remove(this.SelectedLabel);
            }
        }

        private void SelectLabel(MatchLabelViewModel label) {
            if (label.Label != null) {
                this.BytecodeEditor.SelectLabel(label.Label);
            }
        }

        private void EditLabelTarget(MatchLabelViewModel label) {
            if (label.Label != null) {
                // TODO
                // if (this.BytecodeEditor.EditBranchTargetActionWithDialog(out LabelViewModel target)) {
                //     label.Label = target.Label;
                // }
            }
        }
    }
}