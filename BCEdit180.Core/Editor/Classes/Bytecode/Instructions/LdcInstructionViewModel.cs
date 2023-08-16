using System.Collections.Generic;
using System.Windows.Input;
using JavaAsm.Instructions;
using JavaAsm.Instructions.Types;

namespace BCEdit180.Core.Editor.Classes.Bytecode.Instructions {
    public class LdcInstructionViewModel : BaseInstructionViewModel {
        private object value;
        public object Value {
            get => this.value;
            set {
                this.RaisePropertyChanged(ref this.value, value);
                this.IsEditable = value is string;
                if (this.Opcode != Opcode.LDC2_W && (value is long || value is double)) {
                    this.Opcode = Opcode.LDC2_W;
                }
            }
        }

        public override IEnumerable<Opcode> AvailableOpCodes => new Opcode[] {Opcode.LDC, Opcode.LDC_W, Opcode.LDC2_W};

        private bool isEditable;
        public bool IsEditable {
            get => this.isEditable;
            set => this.RaisePropertyChangedIfChanged(ref this.isEditable, value);
        }

        public LdcInstructionViewModel() {
        }

        public override void Load(Instruction instruction) {
            base.Load(instruction);
            LdcInstruction insn = (LdcInstruction) instruction;
            this.Value = insn.Value;
        }

        public override void Save(Instruction instruction) {
            base.Save(instruction);
            LdcInstruction insn = (LdcInstruction) instruction;
            insn.Value = this.Value;
        }
    }
}