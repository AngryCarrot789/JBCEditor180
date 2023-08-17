using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using JavaAsm.Instructions;
using JavaAsm.Instructions.Types;

namespace BCEdit180.Core.Editor.Classes.Bytecode.Instructions {
    public class JumpInstructionViewModel : BaseInstructionViewModel, ILabelTargeter {
        public override IEnumerable<Opcode> AvailableOpCodes => new Opcode[] {Opcode.IFEQ, Opcode.IFNE, Opcode.IFLT, Opcode.IFGE, Opcode.IFGT, Opcode.IFLE, Opcode.IF_ICMPEQ, Opcode.IF_ICMPNE, Opcode.IF_ICMPLT, Opcode.IF_ICMPGE, Opcode.IF_ICMPGT, Opcode.IF_ICMPLE, Opcode.IF_ACMPEQ, Opcode.IF_ACMPNE, Opcode.GOTO, Opcode.JSR, Opcode.IFNULL, Opcode.IFNONNULL};

        private long labelIndex;
        public long LabelIndex {
            get => this.labelIndex;
            set => this.RaisePropertyChanged(ref this.labelIndex, value);
        }

        public ICommand SelectJumpDestinationCommand { get; }

        public override BytecodeEditorViewModel BytecodeEditor {
            set {
                base.BytecodeEditor = value;
                this.EditTargetLabelCommand.RaiseCanExecuteChanged();
            }
        }

        private LabelViewModel targetLabel;
        public LabelViewModel TargetLabel {
            get => this.targetLabel;
            set => this.RaisePropertyChanged(ref this.targetLabel, value);
        }

        private int jumpOffset;
        public int JumpOffset {
            get => this.jumpOffset;
            set => this.RaisePropertyChanged(ref this.jumpOffset, value);
        }

        public AsyncRelayCommand EditTargetLabelCommand { get; }

        public JumpInstructionViewModel() {
            this.SelectJumpDestinationCommand = new RelayCommand(this.SelectJumpDestinationAction);
            this.EditTargetLabelCommand = new AsyncRelayCommand(this.EditTargetLabelAction, () => this.BytecodeEditor != null);
        }

        public async Task EditTargetLabelAction() {
            if (this.BytecodeEditor != null) {
                await this.BytecodeEditor.EditBranchTargetAction(this);
            }
        }

        public void SelectJumpDestinationAction() {
            if (this.BytecodeEditor != null && this.TargetLabel != null) {
                this.BytecodeEditor.PrimarySelectedInstruction = this.TargetLabel;
            }
        }

        public override void Load(Instruction instruction) {
            base.Load(instruction);
            JumpInstruction jump = (JumpInstruction) instruction;
            if (jump.Target != null) {
                this.LabelIndex = jump.Target.Index;
            }
            else {
                this.LabelIndex = -1;
            }

            this.JumpOffset = jump.JumpOffset;
        }

        public override void Save(Instruction instruction) {
            base.Save(instruction);
            JumpInstruction jump = (JumpInstruction) instruction;
            if (this.TargetLabel != null && this.TargetLabel.Instruction != null) {
                jump.Target = this.TargetLabel.Label;
            }
        }
    }
}