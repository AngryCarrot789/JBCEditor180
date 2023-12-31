﻿using System;
using System.Collections.Generic;
using JavaAsm.Helpers;
using JavaAsm.IO.ConstantPoolEntries;

namespace JavaAsm.Instructions.Types {
    public class InvokeDynamicInstruction : Instruction {
        public override Opcode Opcode {
            get => Opcode.INVOKEDYNAMIC;
            set => throw new InvalidOperationException(this.GetType().Name + " only has 1 instruction");
        }

        public override Instruction Copy() {
            return new InvokeDynamicInstruction() {
                Name = this.Name,
                Descriptor = this.Descriptor.CopyMethodDescriptor(),
                BootstrapMethod = this.BootstrapMethod.Copy(),
                BootstrapMethodArgs = new List<object>(this.BootstrapMethodArgs)
            };
        }

        public string Name { get; set; }

        public MethodDescriptor Descriptor { get; set; }

        public Handle BootstrapMethod { get; set; }

        public List<object> BootstrapMethodArgs { get; set; }
    }

    public enum ReferenceKindType : byte {
        GetField = 1,
        GetStatic,
        PutField,
        PutStatic,
        InvokeVirtual,
        InvokeStatic,
        InvokeSpecial,
        NewInvokeSpecial,
        InvokeReference
    }

    public class Handle {
        public ReferenceKindType Type { get; set; }

        public ClassName Owner { get; set; }

        public string Name { get; set; }

        public IDescriptor Descriptor { get; set; }

        internal static Handle FromConstantPool(MethodHandleEntry methodHandleEntry) {
            return new Handle {
                Type = methodHandleEntry.ReferenceKind,
                Descriptor = methodHandleEntry.ReferenceKind.IsFieldReference()
                    ? TypeDescriptor.Parse(methodHandleEntry.Reference.NameAndType.Descriptor.Value)
                    : (IDescriptor) MethodDescriptor.Parse(methodHandleEntry.Reference.NameAndType.Descriptor.Value),
                Name = methodHandleEntry.Reference.NameAndType.Name.Value,
                Owner = new ClassName(methodHandleEntry.Reference.Class.Name.Value)
            };
        }

        internal MethodHandleEntry ToConstantPool() {
            ClassEntry referenceOwner = new ClassEntry(new Utf8Entry(this.Owner.Name));
            NameAndTypeEntry referenceNameAndType = new NameAndTypeEntry(new Utf8Entry(this.Name), new Utf8Entry(this.Descriptor.ToString()));
            ReferenceEntry reference;
            switch (this.Type) {
                case ReferenceKindType.GetField:
                    reference = new FieldReferenceEntry(referenceOwner, referenceNameAndType);
                    break;
                case ReferenceKindType.GetStatic:
                case ReferenceKindType.PutField:
                case ReferenceKindType.PutStatic:
                    reference = new FieldReferenceEntry(referenceOwner, referenceNameAndType);
                    break;
                case ReferenceKindType.InvokeVirtual:
                case ReferenceKindType.NewInvokeSpecial:
                case ReferenceKindType.InvokeStatic:
                case ReferenceKindType.InvokeSpecial:
                    reference = new MethodReferenceEntry(referenceOwner, referenceNameAndType);
                    break;
                case ReferenceKindType.InvokeReference:
                    reference = new InterfaceMethodReferenceEntry(referenceOwner, referenceNameAndType);
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(this.Type));
            }

            return new MethodHandleEntry(this.Type, reference);
        }

        public Handle Copy() {
            return new Handle() {
                Descriptor = this.Descriptor.Copy(),
                Name = this.Name,
                Owner = this.Owner?.Copy(),
                Type = this.Type
            };
        }

        public override string ToString() {
            switch (this.Type) {
                case ReferenceKindType.GetField:
                case ReferenceKindType.GetStatic:
                case ReferenceKindType.PutField:
                case ReferenceKindType.PutStatic:
                    return $"{this.Type} {this.Owner} {this.Name} {this.Descriptor}";
                case ReferenceKindType.InvokeVirtual:
                case ReferenceKindType.InvokeStatic:
                case ReferenceKindType.InvokeSpecial:
                case ReferenceKindType.NewInvokeSpecial:
                case ReferenceKindType.InvokeReference:
                    return $"{this.Type} {this.Owner}.{this.Name}{this.Descriptor}";
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}