using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using BCEdit180.Core.Editor.Classes.Descriptors;
using BCEdit180.Core.Editor.Classes.Utils;
using BCEdit180.Core.Utils;
using JavaAsm;
using JavaAsm.Instructions;
using JavaAsm.Instructions.Types;

namespace BCEdit180.Core.Editor.Classes.Bytecode.Instructions {
    public class InvokeDynamicInstructionViewModel : BaseInstructionViewModel {
        private string name;
        public string Name {
            get => this.name;
            set => this.RaisePropertyChanged(ref this.name, value);
        }

        private MethodDescriptor descriptor;
        public MethodDescriptor Descriptor {
            get => this.descriptor;
            set => this.RaisePropertyChanged(ref this.descriptor, value);
        }

        private ReferenceKindType bootstrapReferenceType;
        public ReferenceKindType BootstrapReferenceType {
            get => this.bootstrapReferenceType;
            set => this.RaisePropertyChanged(ref this.bootstrapReferenceType, value);
        }

        private string bootstrapMethodOwner;
        public string BootstrapMethodOwner {
            get => this.bootstrapMethodOwner;
            set => this.RaisePropertyChanged(ref this.bootstrapMethodOwner, value);
        }

        private string bootstrapMethodName;
        public string BootstrapMethodName {
            get => this.bootstrapMethodName;
            set => this.RaisePropertyChanged(ref this.bootstrapMethodName, value);
        }

        private IDescriptor bootstrapMethodDescriptor;
        public IDescriptor BootstrapMethodDescriptor {
            get => this.bootstrapMethodDescriptor;
            set => this.RaisePropertyChanged(ref this.bootstrapMethodDescriptor, value);
        }

        public ObservableCollection<object> BootstrapMethodArgs { get; }

        public override IEnumerable<Opcode> AvailableOpCodes => new Opcode[] {Opcode.INVOKEDYNAMIC};

        public override bool CanEditOpCode => false;

        public InvokeDynamicInstructionViewModel() {
            this.BootstrapMethodArgs = new ObservableCollection<object>();
        }

        public static object WrapBoostrapArgument(object value) {
            if (value is TypeDescriptor typeDesc) {
                return new TypeDescViewModel() {
                    TypeDescriptor = typeDesc
                };
            }
            else if (value is MethodDescriptor methodDesc) {
                return new MethodDescriptorViewModel() {
                    MethodDescriptor = methodDesc
                };
            }
            else if (value is Handle handle) {
                return new HandleViewModel() {
                    Handle = handle
                };
            }
            else if (value == null) {
                return null;
            }
            else {
                Debug.WriteLine("Cannot wrap invoke dynamic boostrap method argument: " + value.GetType() + " -> " + value);
                return value;
            }
        }

        public static object UnwrapBoostrapArgument(object value) {
            if (value is TypeDescViewModel typeDesc) {
                return typeDesc.TypeDescriptor;
            }
            else if (value is MethodDescriptorViewModel methodDesc) {
                return methodDesc.MethodDescriptor;
            }
            else if (value is HandleViewModel handle) {
                return handle.Handle;
            }
            else if (value == null) {
                return null;
            }
            else {
                Debug.WriteLine("Cannot unwrap invoke dynamic boostrap method argument: " + value.GetType() + " -> " + value);
                return value;
            }
        }

        public override void Load(Instruction instruction) {
            base.Load(instruction);
            InvokeDynamicInstruction insn = (InvokeDynamicInstruction) instruction;
            this.BootstrapMethodArgs.Clear();
            this.Name = insn.Name;
            this.Descriptor = insn.Descriptor;
            if (insn.BootstrapMethod != null) {
                this.BootstrapReferenceType = insn.BootstrapMethod.Type;
                this.BootstrapMethodOwner = insn.BootstrapMethod.Owner.Name;
                this.BootstrapMethodName = insn.BootstrapMethod.Name;
                this.BootstrapMethodDescriptor = insn.BootstrapMethod.Descriptor;
            }

            if (insn.BootstrapMethodArgs != null) {
                this.BootstrapMethodArgs.AddAll(insn.BootstrapMethodArgs.Select(WrapBoostrapArgument));
            }
        }

        public override void Save(Instruction instruction) {
            base.Save(instruction);
            InvokeDynamicInstruction insn = (InvokeDynamicInstruction) instruction;
            insn.Name = this.Name;
            insn.Descriptor = this.Descriptor;
            if (insn.BootstrapMethod == null) {
                insn.BootstrapMethod = new Handle();
            }

            insn.BootstrapMethod.Type = this.BootstrapReferenceType;
            insn.BootstrapMethod.Owner = new ClassName(this.BootstrapMethodOwner);
            insn.BootstrapMethod.Name = this.BootstrapMethodName;
            insn.BootstrapMethod.Descriptor = this.BootstrapMethodDescriptor;
            insn.BootstrapMethodArgs = this.BootstrapMethodArgs.Select(UnwrapBoostrapArgument).ToList();

            // switch (this.BootstrapReferenceType) {
            //     case ReferenceKindType.GetField:
            //     case ReferenceKindType.GetStatic:
            //     case ReferenceKindType.PutField:
            //     case ReferenceKindType.PutStatic:
            //         insn.BootstrapMethod.Descriptor = TypeDescriptor.Parse(this.BootstrapMethodDescriptor);
            //         break;
            //     case ReferenceKindType.InvokeVirtual:
            //     case ReferenceKindType.InvokeStatic:
            //     case ReferenceKindType.InvokeSpecial:
            //     case ReferenceKindType.NewInvokeSpecial:
            //     case ReferenceKindType.InvokeReference:
            //         insn.BootstrapMethod.Descriptor = MethodDescriptor.Parse(this.BootstrapMethodDescriptor);
            //         break;
            //     default: throw new Exception("Unknown bootstrap reference type: " + this.BootstrapReferenceType);
            // }
        }
    }
}