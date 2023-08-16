using System.Collections.Generic;
using BCEdit180.Core.Editor.Classes.Methods;
using JavaAsm;
using JavaAsm.Instructions;
using JavaAsm.Instructions.Types;

namespace BCEdit180.Core.Editor.Classes.Bytecode.Instructions {
    public class MethodInstructionViewModel : BaseInstructionViewModel, IMethodDescriptable {
        private string methodOwner;
        public string MethodOwner {
            get => this.methodOwner;
            set => this.RaisePropertyChanged(ref this.methodOwner, value);
        }

        private string methodName;
        public string MethodName {
            get => this.methodName;
            set => this.RaisePropertyChanged(ref this.methodName, value);
        }

        private MethodDescriptor methodDescriptor;
        public MethodDescriptor MethodDescriptor {
            get => this.methodDescriptor;
            set => this.RaisePropertyChanged(ref this.methodDescriptor, value);
        }

        public override IEnumerable<Opcode> AvailableOpCodes => new Opcode[] {Opcode.INVOKESTATIC, Opcode.INVOKEVIRTUAL, Opcode.INVOKEINTERFACE, Opcode.INVOKESPECIAL};

        public MethodInstructionViewModel() {
        }

        public override void Load(Instruction instruction) {
            base.Load(instruction);
            MethodInstruction insn = (MethodInstruction) instruction;
            this.MethodOwner = insn.Owner.Name;
            this.MethodName = insn.Name;
            this.MethodDescriptor = insn.Descriptor;
        }

        public override void Save(Instruction instruction) {
            base.Save(instruction);
            MethodInstruction insn = (MethodInstruction) instruction;
            insn.Owner = new ClassName(this.MethodOwner);
            insn.Name = this.MethodName;
            insn.Descriptor = this.MethodDescriptor;
        }
    }
}