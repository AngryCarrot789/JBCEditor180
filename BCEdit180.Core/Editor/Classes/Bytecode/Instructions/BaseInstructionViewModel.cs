﻿using System;
using System.Collections.Generic;
using System.Windows.Input;
using JavaAsm.Instructions;
using JavaAsm.Instructions.Types;

namespace BCEdit180.Core.Editor.Classes.Bytecode.Instructions {
    public abstract class BaseInstructionViewModel : BaseViewModel {
        private Opcode opcode;
        private bool isNewInstruction;
        private bool isEnabled;
        private int instructionIndex;
        private BytecodeEditorViewModel bytecodeEditor;

        public Instruction Instruction { get; protected set; }

        public abstract IEnumerable<Opcode> AvailableOpCodes { get; }

        public Opcode Opcode {
            get => this.opcode;
            set => this.RaisePropertyChanged(ref this.opcode, value);
        }

        public bool IsNewInstruction {
            get => this.isNewInstruction;
            set => this.RaisePropertyChanged(ref this.isNewInstruction, value);
        }

        public virtual bool CanEditOpCode => true;

        public bool IsEnabled {
            get => this.isEnabled;
            set => this.RaisePropertyChanged(ref this.isEnabled, value);
        }

        public int InstructionIndex {
            get => this.instructionIndex;
            set => this.RaisePropertyChanged(ref this.instructionIndex, value);
        }

        /// <summary>
        /// The editor that this instruction is currently placed in
        /// </summary>
        public virtual BytecodeEditorViewModel BytecodeEditor {
            get => this.bytecodeEditor;
            set => this.RaisePropertyChanged(ref this.bytecodeEditor, value);
        }

        public Action<BaseInstructionViewModel> DuplicateCallback { get; set; }
        public Action<BaseInstructionViewModel> RemoveSelfCallback { get; set; }

        public ICommand EditOpcodeCommand { get; }
        public ICommand DuplicateCommand { get; }
        public ICommand RemoveSelfCommand { get; }

        protected BaseInstructionViewModel() {
            this.EditOpcodeCommand = new RelayCommand(this.EditOpcode, () => this.CanEditOpCode);
            this.DuplicateCommand = new RelayCommand(this.DuplicateAction);
            this.RemoveSelfCommand = new RelayCommand(this.RemoveSelfAction);
            this.IsEnabled = true;
        }

        private void RemoveSelfAction() {
            this.RemoveSelfCallback?.Invoke(this);
        }

        public void DuplicateAction() {
            this.DuplicateCallback?.Invoke(this);
        }

        // Always check CanEditOpCode
        public void EditOpcode() {
            // if (this.CanEditOpCode && Dialogs.TypeEditor.ChangeInstructionDialog(this.AvailableOpCodes, this.Opcode, out Opcode code)) {
            //     this.Opcode = code;
            // }
        }

        public static BaseInstructionViewModel ForInstruction(Instruction instruction) {
            if (instruction == null) {
                throw new NullReferenceException("Instruction cannot be null");
            }
            else if (instruction is FieldInstruction) {
                return new FieldInstructionViewModel();
            }
            else if (instruction is IncrementInstruction) {
                return new IncrementInstructionViewModel();
            }
            else if (instruction is IntegerPushInstruction) {
                return new IntegerPushInstructionViewModel();
            }
            else if (instruction is InvokeDynamicInstruction) {
                return new InvokeDynamicInstructionViewModel();
            }
            else if (instruction is JumpInstruction) {
                return new JumpInstructionViewModel();
            }
            else if (instruction is Label) {
                return new LabelViewModel();
            }
            else if (instruction is LdcInstruction) {
                return new LdcInstructionViewModel();
            }
            else if (instruction is LineNumber) {
                return new LineNumberViewModel();
            }
            else if (instruction is LookupSwitchInstruction) {
                return new LookupSwitchInstructionViewModel();
            }
            else if (instruction is MethodInstruction) {
                return new MethodInstructionViewModel();
            }
            else if (instruction is MultiANewArrayInstruction) {
                return new MultiANewArrayInstructionViewModel();
            }
            else if (instruction is NewArrayInstruction) {
                return new NewArrayInstructionViewModel();
            }
            else if (instruction is SimpleInstruction) {
                return new SimpleInstructionViewModel();
            }
            else if (instruction is StackMapFrame) {
                return new StackMapFrameViewModel();
            }
            else if (instruction is TableSwitchInstruction) {
                return new TableSwitchInstructionViewModel();
            }
            else if (instruction is TypeInstruction) {
                return new TypeInstructionViewModel();
            }
            else if (instruction is VariableInstruction) {
                return new VariableInstructionViewModel();
            }
            else {
                throw new ArgumentException("Unexpected instruction: " + instruction.GetType() + " -> " + instruction);
            }
        }

        public virtual void Load(Instruction instruction) {
            this.Instruction = instruction;
            this.Opcode = instruction.Opcode;
        }

        public virtual void Save(Instruction instruction) {
            if (this.CanEditOpCode) {
                instruction.Opcode = this.Opcode;
            }
        }

        public override string ToString() {
            return this.Instruction?.ToString() ?? $"[Instruction handle unavailable. Opcode = {this.Opcode}]";
        }

        protected static T Require<T>(Instruction instruction) where T : Instruction {
            if (instruction is T t) {
                return t;
            }

            throw new InvalidOperationException($"This function requires {typeof(T).Name} but {instruction?.GetType()?.Name ?? "null"} was used instead");
        }

        public virtual bool OnMoving(int curIndex, int newIndex) {
            return true;
        }
    }
}