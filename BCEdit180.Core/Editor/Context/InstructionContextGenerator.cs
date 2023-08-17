using System.Collections.Generic;
using BCEdit180.Core.Actions.Contexts;
using BCEdit180.Core.AdvancedContextService;
using BCEdit180.Core.Editor.Classes.Bytecode;
using BCEdit180.Core.Editor.Classes.Bytecode.Instructions;

namespace BCEdit180.Core.Editor.Context {
    public class InstructionContextGenerator : IContextGenerator {
        public static InstructionContextGenerator Instance { get; } = new InstructionContextGenerator();

        private InstructionContextGenerator() {
        }

        public void Generate(List<IContextEntry> list, IDataContext context) {
            if (context.TryGetContext(out BaseInstructionViewModel instruction)) {
                list.Add(new CommandContextEntry("Change Opcode", instruction.EditOpcodeCommand));
                if (context.TryGetContext(out BytecodeEditorViewModel editor) || (editor = instruction.BytecodeEditor) != null) {
                    list.Add(new CommandContextEntry("Duplicate", editor.DuplicateItemCommand, instruction));
                    list.Add(SeparatorEntry.Instance);
                    list.Add(new CommandContextEntry("Remove", editor.RemoveItemCommand, instruction));
                }
            }
        }
    }
}