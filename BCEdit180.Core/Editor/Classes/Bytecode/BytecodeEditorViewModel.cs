﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using BCEdit180.Core.Editor.Classes.Bytecode.Instructions;
using BCEdit180.Core.Utils;
using JavaAsm;
using JavaAsm.Instructions;
using JavaAsm.Instructions.Types;

namespace BCEdit180.Core.Editor.Classes.Bytecode {
    public class BytecodeEditorViewModel : BaseViewModel {
        public ObservableCollectionEx<BaseInstructionViewModel> Instructions { get; }
        public ObservableCollectionEx<BaseInstructionViewModel> RemovedInstructions { get; }

        private int selectedInstructionIndex;
        public int SelectedInstructionIndex {
            get => this.selectedInstructionIndex;
            set => this.RaisePropertyChanged(ref this.selectedInstructionIndex, value);
        }

        private BaseInstructionViewModel primarySelectedInstruction;
        public BaseInstructionViewModel PrimarySelectedInstruction {
            get => this.primarySelectedInstruction;
            set => this.RaisePropertyChanged(ref this.primarySelectedInstruction, value);
        }

        public ObservableCollection<BaseInstructionViewModel> SelectedInstructions { get; }

        public RelayCommand<BaseInstructionViewModel> RemoveItemCommand { get; }

        public RelayCommand<BaseInstructionViewModel> DuplicateItemCommand { get; }

        public ICommand DeleteSelectedInstructionsCommand { get; }
        public ICommand InsertInstructionCommand { get; }
        public ICommand InsertCodeCommand { get; }
        public ICommand InsertLabelCommand { get; }

        public AsyncRelayCommand MoveSelectedInstructionUpCommand { get; }
        public AsyncRelayCommand MoveSelectedInstructionDownCommand { get; }

        public ICommand CopyCodeCommand { get; }

        public ICommand PasteCodeAboveCommand { get; }
        public ICommand PasteCodeBelowCommand { get; }

        public CodeEditorViewModel CodeEditor { get; }

        private int nextInstructionIndex;

        public static readonly Opcode[] ALL_OPCODES;

        static BytecodeEditorViewModel() {
            List<Opcode> opcodes = new List<Opcode>(256);
            foreach (object opcode in Enum.GetValues(typeof(Opcode))) {
                opcodes.Add((Opcode) opcode);
            }

            ALL_OPCODES = opcodes.ToArray();
        }

        public BytecodeEditorViewModel(CodeEditorViewModel codeEditor) {
            this.CodeEditor = codeEditor ?? throw new ArgumentNullException(nameof(codeEditor));
            this.Instructions = new ObservableCollectionEx<BaseInstructionViewModel>();
            this.SelectedInstructions = new ObservableCollection<BaseInstructionViewModel>();
            this.RemovedInstructions = new ObservableCollectionEx<BaseInstructionViewModel>();
            this.DeleteSelectedInstructionsCommand = new RelayCommand(this.DeleteSelectedInstructions);
            this.InsertCodeCommand = new RelayCommand(() => {
                // this.Instructions.Add(new FieldInstructionViewModel() {
                //     FieldName = "TestField101", Opcode = Opcode.GETFIELD, FieldOwner = "Joe", IsNewInstruction = true, FieldDescriptor = new TypeDescriptor(PrimitiveType.Boolean, 0)
                // });
            });

            this.InsertInstructionCommand = new RelayCommand(this.InsertInstructionAction);
            this.MoveSelectedInstructionUpCommand = new AsyncRelayCommand(this.MoveSelectedInstructionUpAction);
            this.MoveSelectedInstructionDownCommand = new AsyncRelayCommand(this.MoveSelectedInstructionDownAction);
            this.InsertLabelCommand = new RelayCommand(this.InsertLabelAction);
            this.CopyCodeCommand = new RelayCommand(this.CopyCodeAction);
            this.PasteCodeAboveCommand = new RelayCommand(() => this.PasteCodeAction(false));
            this.PasteCodeBelowCommand = new RelayCommand(() => this.PasteCodeAction(true));
            this.RemoveItemCommand = new RelayCommand<BaseInstructionViewModel>(this.RemoveSpecificInstruction);
            this.DuplicateItemCommand = new RelayCommand<BaseInstructionViewModel>(this.DuplicateInstructionAbove);
        }

        private void PasteCodeAction(bool below) {
            // int index = this.SelectedInstruction == null ? 0 : this.SelectedInstructionIndex;
            // if (below && (index + 1) <= this.Instructions.Count) {
            //     index++;
            // }
            // List<BaseInstructionViewModel> list = IoC.Instance.Provide<InstructionClipboardViewModel>().Instructions.ToList();
            // if (list.Count > 0) {
            //     foreach (BaseInstructionViewModel item in list) {
            //         this.SetupCallbacks(item);
            //         item.IsNewInstruction = true;
            //     }
            //     this.Instructions.InsertRange(index, list.OrderBy(i => i.InstructionIndex));
            // }
            // this.CalculateInstructionIndices();
            // this.ResolveBranchLabels();
        }

        private void CopyCodeAction() {
            // List<BaseInstructionViewModel> list = BytecodeList.SelectedItems.ToList();
            // if (list.Count > 0) {
            //     IoC.Instance.Provide<InstructionClipboardViewModel>().SetClipboard(list);
            // }
        }

        private void InsertLabelAction() {
            bool insert = false;
            Label instruction = new Label();
            if (this.PrimarySelectedInstruction?.Instruction != null) {
                insert = true;
            }

            BaseInstructionViewModel instructionVM = this.CreateInstructionForHandle(instruction);
            instructionVM.IsNewInstruction = true;
            if (insert) {
                this.Instructions.Insert(this.SelectedInstructionIndex, instructionVM);
            }
            else {
                this.Instructions.Add(instructionVM);
            }

            this.CalculateInstructionIndices();
        }

        private async Task MoveSelectedInstructionUpAction() {
            List<int> indices = this.SelectedInstructions.Select(x => this.Instructions.IndexOf(x)).ToList();
            if (indices.Any(x => x < 1)) {
                return;
            }

            // Sort small to big. This is required otherwise move operations will not work
            indices.Sort((a, b) => a > b ? 1 : a < b ? -1 : throw new Exception("Duplicate index"));
            foreach (int index in indices) {
                int newIndex = index - 1;
                BaseInstructionViewModel curInstruction = this.Instructions[index];
                BaseInstructionViewModel newInstruction = this.Instructions[newIndex];
                if (await this.CanMoveInstructions(index, newIndex, curInstruction, newInstruction)) {
                    curInstruction.InstructionIndex--;
                    newInstruction.InstructionIndex++;
                    this.Instructions.Move(index, newIndex);
                }
            }
        }

        private async Task MoveSelectedInstructionDownAction() {
            int lastIndex = this.Instructions.Count - 1;
            List<int> indices = this.SelectedInstructions.Select(x => this.Instructions.IndexOf(x)).ToList();
            if (indices.Any(x => x == -1 || x == lastIndex)) {
                return;
            }

            // Sort big to small. This is required otherwise move operations will not work
            indices.Sort((a, b) => a > b ? -1 : a < b ? 1 : throw new Exception("Duplicate index"));
            foreach (int index in indices) {
                int newIndex = index + 1;
                BaseInstructionViewModel curInstruction = this.Instructions[index];
                BaseInstructionViewModel newInstruction = this.Instructions[newIndex];
                if (await this.CanMoveInstructions(index, newIndex, curInstruction, newInstruction)) {
                    curInstruction.InstructionIndex++;
                    newInstruction.InstructionIndex--;
                    this.Instructions.Move(index, newIndex);
                }
            }
        }

        private async Task<bool> CanMoveInstructions(int curIndex, int newIndex, BaseInstructionViewModel curInsn, BaseInstructionViewModel newInsn) {
            if (curInsn.OnMoving(curIndex, newIndex)) {
                if (newInsn.OnMoving(newIndex, curIndex)) {
                    return true;
                }
                else {
                    await IoC.MessageDialogs.ShowMessageAsync("Immovable instruction", $"The instruction ({(curIndex > newIndex ? "above" : "below")}) the selected instruction cannot be moved: " + newInsn.Opcode);
                }
            }
            else {
                await IoC.MessageDialogs.ShowMessageAsync("Immovable instruction", "The selected instruction cannot be moved: " + curInsn.Opcode);
            }

            return false;
        }

        public void SelectLabel(Label label) {
            for (int i = 0; i < this.Instructions.Count; i++) {
                if (this.Instructions[i] is LabelViewModel labelVM && labelVM.Label == label) {
                    this.SelectAndScrollToInstruction(i);
                    return;
                }
            }
        }

        public void SelectAndScrollToInstruction(int index) {
            this.SelectedInstructionIndex = index;
            // if (this.SelectedInstruction != null) {
            //     BytecodeList.ScrollToSelectedItem();
            // }
        }

        public void SelectAndScrollToInstruction(BaseInstructionViewModel instruction) {
            this.PrimarySelectedInstruction = instruction;
            // if (this.SelectedInstruction == instruction) {
            //     BytecodeList.ScrollToSelectedItem();
            // }
        }

        public async Task EditBranchTargetAction(JumpInstructionViewModel jump) {
            if (!this.Instructions.Any(x => x is LabelViewModel)) {
                await IoC.MessageDialogs.ShowMessageAsync("No labels", "There are no labels to jump to");
                return;
            }

            await IoC.MessageDialogs.ShowMessageAsync("Not implemented", "This feature is yet to be implemented");
            // jump.TargetLabel = label;
            // this.PrimarySelectedInstruction = label;
            // jump.JumpOffset = -1;
            // jump.LabelIndex = label.Index;
        }

        public void InsertInstructionAction() {
            // TODO
            // if (IoC.TypeEditor.ChangeInstructionDialog(ALL_OPCODES, out Opcode opcode)) {
            //     Instruction instruction = null;
            //     Type instructionType = GetInstructionTypeForOpCode(opcode);
            //     if (instructionType == null) {
            //         IoC.MessageDialogs.ShowMessage("Unknown instruction", $"The opcode '{opcode}' is unrecognised");
            //         return;
            //     }
            //     if (instructionType == typeof(FieldInstruction)) {
            //         instruction = new FieldInstruction(opcode) {
            //             Descriptor = new TypeDescriptor(PrimitiveType.Integer, 0),
            //             Name = "newField",
            //             Owner = new ClassName(this.CodeEditor.MethodInfo.MethodList.Class.ClassInfo.FullClassName)
            //         };
            //     }
            //     else if (instructionType == typeof(IncrementInstruction)) {
            //         instruction = new IncrementInstruction();
            //     }
            //     else if (instructionType == typeof(IntegerPushInstruction)) {
            //         instruction = new IntegerPushInstruction(opcode);
            //     }
            //     else if (instructionType == typeof(InvokeDynamicInstruction)) {
            //         instruction = new InvokeDynamicInstruction() {
            //             Descriptor = new MethodDescriptor(new TypeDescriptor(PrimitiveType.Integer, 0)),
            //             Name = "invokeDynamicMethod"
            //         };
            //     }
            //     else if (instructionType == typeof(JumpInstruction)) {
            //         instruction = new JumpInstruction(opcode);
            //     }
            //     else if (instructionType == typeof(Label)) {
            //         instruction = new Label();
            //     }
            //     else if (instructionType == typeof(LdcInstruction)) {
            //         instruction = new LdcInstruction(opcode);
            //     }
            //     else if (instructionType == typeof(LineNumber)) {
            //         instruction = new LineNumber();
            //     }
            //     else if (instructionType == typeof(LookupSwitchInstruction)) {
            //         instruction = new LookupSwitchInstruction();
            //     }
            //     else if (instructionType == typeof(MethodInstruction)) {
            //         instruction = new MethodInstruction(opcode) {
            //             Descriptor = new MethodDescriptor(new TypeDescriptor(PrimitiveType.Integer, 0)),
            //             Name = "newMethod",
            //             Owner = new ClassName(this.CodeEditor.MethodInfo.MethodList.Class.ClassInfo.FullClassName)
            //         };
            //     }
            //     else if (instructionType == typeof(MultiANewArrayInstruction)) {
            //         instruction = new MultiANewArrayInstruction() {
            //             Dimensions = 1,
            //             Type = new ClassName("java/lang/String")
            //         };
            //     }
            //     else if (instructionType == typeof(NewArrayInstruction)) {
            //         instruction = new NewArrayInstruction() {
            //             ArrayType = NewArrayTypeCode.Integer
            //         };
            //     }
            //     else if (instructionType == typeof(SimpleInstruction)) {
            //         instruction = new SimpleInstruction(opcode);
            //     }
            //     else if (instructionType == typeof(StackMapFrame)) {
            //         instruction = new StackMapFrame();
            //     }
            //     else if (instructionType == typeof(TableSwitchInstruction)) {
            //         instruction = new TableSwitchInstruction();
            //     }
            //     else if (instructionType == typeof(TypeInstruction)) {
            //         instruction = new TypeInstruction(opcode);
            //     }
            //     else if (instructionType == typeof(VariableInstruction)) {
            //         instruction = new VariableInstruction(opcode);
            //     }
            //     else {
            //         IoC.MessageDialogs.ShowMessage("Failed to create instruction", "Failed to create instruction for opcode: " + opcode);
            //         return;
            //     }
            //     bool insert = false;
            //     instruction.Opcode = opcode;
            //     if (this.SelectedInstruction != null && this.SelectedInstruction.Instruction != null) {
            //         this.CodeEditor.MethodInfo.Node.Instructions.InsertBefore(this.SelectedInstruction.Instruction, instruction);
            //         insert = true;
            //     }
            //     else {
            //         this.CodeEditor.MethodInfo.Node.Instructions.Add(instruction);
            //     }
            //     BaseInstructionViewModel instructionViewModel = this.CreateInstructionForHandle(instruction);
            //     instructionViewModel.IsNewInstruction = true;
            //     if (insert) {
            //         this.Instructions.Insert(this.SelectedInstructionIndex, instructionViewModel);
            //     }
            //     else {
            //         this.Instructions.Add(instructionViewModel);
            //     }
            //     this.CalculateInstructionIndices();
            // }
        }

        public void DeleteSelectedInstructions() {
            // List<BaseInstructionViewModel> instructions = new List<BaseInstructionViewModel>(BytecodeList.SelectedItems);
            // this.RemovedInstructions.AddRange(instructions);
            // this.Instructions.RemoveRange(instructions);
            // this.CalculateInstructionIndices();
        }

        public void RemoveInstruction(BaseInstructionViewModel instruction) {
            List<BaseInstructionViewModel> selected = this.SelectedInstructions.ToList();
            if (selected.Count > 1) {
                this.Instructions.RemoveRange(selected);
            }
            else {
                int index = this.SelectedInstructionIndex - 1;
                if (index < 0) {
                    index = 0;
                }

                this.Instructions.Remove(instruction);
                if (this.Instructions.Count > 0) {
                    this.SelectedInstructionIndex = index;
                }
            }

            this.CalculateInstructionIndices();
        }

        public void RemoveSpecificInstruction(BaseInstructionViewModel instruction) {
            int index = this.SelectedInstructionIndex - 1;
            if (index < 0) {
                index = 0;
            }

            this.Instructions.Remove(instruction);

            if (this.Instructions.Count > 0) {
                this.SelectedInstructionIndex = index;
            }

            this.CalculateInstructionIndices();
        }

        public void DuplicateInstructionAbove(BaseInstructionViewModel instruction) {
            int index = this.Instructions.IndexOf(instruction);
            if (index == -1 || instruction.Instruction == null) {
                return;
            }

            Instruction instructionHandle = instruction.Instruction.Copy();
            instruction.Save(instructionHandle);
            BaseInstructionViewModel copy = this.CreateInstructionForHandle(instructionHandle);
            copy.IsNewInstruction = true;
            this.Instructions.Insert(index, copy);
            this.CalculateInstructionIndices();
        }

        public BaseInstructionViewModel CreateInstructionForHandle(Instruction instruction, int index = -1) {
            BaseInstructionViewModel vm = BaseInstructionViewModel.ForInstruction(instruction);
            vm.BytecodeEditor = this;
            vm.InstructionIndex = index != -1 ? index : this.nextInstructionIndex++;
            vm.Load(instruction);
            return vm;
        }

        public void ResolveBranchLabels() {
            Dictionary<long, LabelViewModel> labelMap = new Dictionary<long, LabelViewModel>();
            foreach(BaseInstructionViewModel instruction in this.Instructions) {
                if (instruction is LabelViewModel label && label.Instruction != null) {
                    labelMap[label.Index] = label;
                }
            }

            foreach(BaseInstructionViewModel instruction in this.Instructions) {
                if (instruction is JumpInstructionViewModel jump) {
                    if (labelMap.TryGetValue(jump.LabelIndex, out LabelViewModel label)) {
                        jump.TargetLabel = label;
                    }
                    else {
                        jump.TargetLabel = null;
                        jump.LabelIndex = -1;
                    }
                }
            }
        }

        public void Load(MethodNode node) {
            this.RemovedInstructions.Clear();
            this.Instructions.Clear();
            this.nextInstructionIndex = 0;
            Dictionary<long, LabelViewModel> labelMap = new Dictionary<long, LabelViewModel>();
            List<BaseInstructionViewModel> instructions = new List<BaseInstructionViewModel>();
            foreach(Instruction instruction in node.Instructions) {
                BaseInstructionViewModel baseInstruction = this.CreateInstructionForHandle(instruction);
                if (baseInstruction is LabelViewModel label) {
                    labelMap[label.Index] = label;
                }

                instructions.Add(baseInstruction);
            }

            foreach(BaseInstructionViewModel instruction in instructions) {
                if (instruction is JumpInstructionViewModel jump) {
                    if (labelMap.TryGetValue(jump.LabelIndex, out LabelViewModel label)) {
                        jump.TargetLabel = label;
                    }
                    else {
                        jump.TargetLabel = null;
                        jump.LabelIndex = -1;
                    }
                }
            }

            this.Instructions.AddRange(instructions);
            // List<BaseInstructionViewModel> instructions = new List<BaseInstructionViewModel>(node.Instructions.Count);
            // for (Instruction instruction = node.Instructions.First; instruction != null; instruction = instruction.Next) {
            //     instructions.Add(CreateInstructionForHandle(instruction));
            // }
        }

        public void Save(MethodNode node) {
            node.Instructions = new InstructionList();
            this.RemovedInstructions.Clear();
            foreach (BaseInstructionViewModel instruction in this.Instructions) {
                instruction.Save(instruction.Instruction);
            }

            // juuust in case...
            // CalculateInstructionIndices();
            int lastIndex = -1;
            foreach (BaseInstructionViewModel viewModel in this.Instructions) {
                if ((viewModel.InstructionIndex - 1) != lastIndex) {
                    // IoC.MessageDialogs.ShowMessage("Corrupt instruction order","Bug: some of the instruction indices were out of order. This won't corrupt the class file or anything, as these indices are used for visual help");
                    this.CalculateInstructionIndices();
                    break;
                }

                lastIndex = viewModel.InstructionIndex;
            }

            // i trust the order will not be corrupt, because it can't get corrupt unless there's a binding failure...
            foreach (BaseInstructionViewModel instruction in this.Instructions) {
                node.Instructions.Add(instruction.Instruction);
            }
        }

        public void CalculateInstructionIndices(int startIndex = 0) {
            Collection<BaseInstructionViewModel> instructions = this.Instructions;
            for (int i = startIndex; i < instructions.Count; i++) {
                instructions[i].InstructionIndex = i;
            }

            this.nextInstructionIndex = instructions.Count;
        }

        public static Type GetInstructionTypeForOpCode(Opcode opcode) {
            switch (opcode) {
                case Opcode.NOP:
                case Opcode.ACONST_NULL:
                case Opcode.ICONST_M1:
                case Opcode.ICONST_0:
                case Opcode.ICONST_1:
                case Opcode.ICONST_2:
                case Opcode.ICONST_3:
                case Opcode.ICONST_4:
                case Opcode.ICONST_5:
                case Opcode.LCONST_0:
                case Opcode.LCONST_1:
                case Opcode.FCONST_0:
                case Opcode.FCONST_1:
                case Opcode.FCONST_2:
                case Opcode.DCONST_0:
                case Opcode.DCONST_1:
                case Opcode.IALOAD:
                case Opcode.LALOAD:
                case Opcode.FALOAD:
                case Opcode.DALOAD:
                case Opcode.AALOAD:
                case Opcode.BALOAD:
                case Opcode.CALOAD:
                case Opcode.SALOAD:
                case Opcode.IASTORE:
                case Opcode.LASTORE:
                case Opcode.FASTORE:
                case Opcode.DASTORE:
                case Opcode.AASTORE:
                case Opcode.BASTORE:
                case Opcode.CASTORE:
                case Opcode.SASTORE:
                case Opcode.POP:
                case Opcode.POP2:
                case Opcode.DUP:
                case Opcode.DUP_X1:
                case Opcode.DUP_X2:
                case Opcode.DUP2:
                case Opcode.DUP2_X1:
                case Opcode.DUP2_X2:
                case Opcode.SWAP:
                case Opcode.IADD:
                case Opcode.LADD:
                case Opcode.FADD:
                case Opcode.DADD:
                case Opcode.ISUB:
                case Opcode.LSUB:
                case Opcode.FSUB:
                case Opcode.DSUB:
                case Opcode.IMUL:
                case Opcode.LMUL:
                case Opcode.FMUL:
                case Opcode.DMUL:
                case Opcode.IDIV:
                case Opcode.LDIV:
                case Opcode.FDIV:
                case Opcode.DDIV:
                case Opcode.IREM:
                case Opcode.LREM:
                case Opcode.FREM:
                case Opcode.DREM:
                case Opcode.INEG:
                case Opcode.LNEG:
                case Opcode.FNEG:
                case Opcode.DNEG:
                case Opcode.ISHL:
                case Opcode.LSHL:
                case Opcode.ISHR:
                case Opcode.LSHR:
                case Opcode.IUSHR:
                case Opcode.LUSHR:
                case Opcode.IAND:
                case Opcode.LAND:
                case Opcode.IOR:
                case Opcode.LOR:
                case Opcode.IXOR:
                case Opcode.LXOR:
                case Opcode.I2L:
                case Opcode.I2F:
                case Opcode.I2D:
                case Opcode.L2I:
                case Opcode.L2F:
                case Opcode.L2D:
                case Opcode.F2I:
                case Opcode.F2L:
                case Opcode.F2D:
                case Opcode.D2I:
                case Opcode.D2L:
                case Opcode.D2F:
                case Opcode.I2B:
                case Opcode.I2C:
                case Opcode.I2S:
                case Opcode.LCMP:
                case Opcode.FCMPL:
                case Opcode.FCMPG:
                case Opcode.DCMPL:
                case Opcode.DCMPG:
                case Opcode.IRETURN:
                case Opcode.LRETURN:
                case Opcode.FRETURN:
                case Opcode.DRETURN:
                case Opcode.ARETURN:
                case Opcode.RETURN:
                case Opcode.ARRAYLENGTH:
                case Opcode.ATHROW:
                case Opcode.MONITORENTER:
                case Opcode.MONITOREXIT:
                    return typeof(SimpleInstruction);
                case Opcode.BIPUSH:
                case Opcode.SIPUSH:
                    return typeof(IntegerPushInstruction);
                case Opcode.LDC:
                case Opcode.LDC_W:
                case Opcode.LDC2_W:
                    return typeof(LdcInstruction);
                case Opcode.ILOAD:
                case Opcode.LLOAD:
                case Opcode.FLOAD:
                case Opcode.DLOAD:
                case Opcode.ALOAD:
                case Opcode.ISTORE:
                case Opcode.LSTORE:
                case Opcode.FSTORE:
                case Opcode.DSTORE:
                case Opcode.ASTORE:
                case Opcode.RET:
                case Opcode.ILOAD_0:
                case Opcode.ILOAD_1:
                case Opcode.ILOAD_2:
                case Opcode.ILOAD_3:
                case Opcode.LLOAD_0:
                case Opcode.LLOAD_1:
                case Opcode.LLOAD_2:
                case Opcode.LLOAD_3:
                case Opcode.FLOAD_0:
                case Opcode.FLOAD_1:
                case Opcode.FLOAD_2:
                case Opcode.FLOAD_3:
                case Opcode.DLOAD_0:
                case Opcode.DLOAD_1:
                case Opcode.DLOAD_2:
                case Opcode.DLOAD_3:
                case Opcode.ALOAD_0:
                case Opcode.ALOAD_1:
                case Opcode.ALOAD_2:
                case Opcode.ALOAD_3:
                case Opcode.ISTORE_0:
                case Opcode.ISTORE_1:
                case Opcode.ISTORE_2:
                case Opcode.ISTORE_3:
                case Opcode.LSTORE_0:
                case Opcode.LSTORE_1:
                case Opcode.LSTORE_2:
                case Opcode.LSTORE_3:
                case Opcode.FSTORE_0:
                case Opcode.FSTORE_1:
                case Opcode.FSTORE_2:
                case Opcode.FSTORE_3:
                case Opcode.DSTORE_0:
                case Opcode.DSTORE_1:
                case Opcode.DSTORE_2:
                case Opcode.DSTORE_3:
                case Opcode.ASTORE_0:
                case Opcode.ASTORE_1:
                case Opcode.ASTORE_2:
                case Opcode.ASTORE_3:
                    return typeof(VariableInstruction);
                case Opcode.IINC: return typeof(IncrementInstruction);
                case Opcode.IFEQ:
                case Opcode.IFNE:
                case Opcode.IFLT:
                case Opcode.IFGE:
                case Opcode.IFGT:
                case Opcode.IFLE:
                case Opcode.IF_ICMPEQ:
                case Opcode.IF_ICMPNE:
                case Opcode.IF_ICMPLT:
                case Opcode.IF_ICMPGE:
                case Opcode.IF_ICMPGT:
                case Opcode.IF_ICMPLE:
                case Opcode.IF_ACMPEQ:
                case Opcode.IF_ACMPNE:
                case Opcode.GOTO:
                case Opcode.JSR:
                case Opcode.IFNULL:
                case Opcode.IFNONNULL:
                    return typeof(JumpInstruction);
                case Opcode.TABLESWITCH: return typeof(TableSwitchInstruction);
                case Opcode.LOOKUPSWITCH: return typeof(LookupSwitchInstruction);
                case Opcode.GETSTATIC:
                case Opcode.PUTSTATIC:
                case Opcode.GETFIELD:
                case Opcode.PUTFIELD:
                    return typeof(FieldInstruction);
                case Opcode.INVOKEVIRTUAL:
                case Opcode.INVOKESPECIAL:
                case Opcode.INVOKESTATIC:
                case Opcode.INVOKEINTERFACE:
                    return typeof(MethodInstruction);
                case Opcode.INVOKEDYNAMIC: return typeof(InvokeDynamicInstruction);
                case Opcode.NEW:
                case Opcode.ANEWARRAY:
                case Opcode.CHECKCAST:
                case Opcode.INSTANCEOF:
                    return typeof(TypeInstruction);
                case Opcode.NEWARRAY: return typeof(NewArrayInstruction);
                case Opcode.MULTIANEWARRAY: return typeof(MultiANewArrayInstruction);
                case Opcode.GOTO_W:
                case Opcode.JSR_W:
                    return typeof(JumpInstruction);
                default: throw new ArgumentException(nameof(opcode), "Unsupported opcode: " + opcode);
            }
        }

        public void VerifyDuplicateLabelIndex(LabelViewModel label) {
            // foreach (BaseInstructionViewModel item in this.Instructions) {
            //     if (item is LabelViewModel labelItem) {
            //         if (labelItem.Index == label.Index && labelItem != label) {
            //             // IoC.MessageDialogs.ShowMessage("Index already in use", $"The label at line {labelItem.InstructionIndex} is already using the label index {labelItem.Index}");
            //             return;
            //         }
            //     }
            // }
        }
    }
}