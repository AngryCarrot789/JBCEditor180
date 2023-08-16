using System;
using BCEdit180.Core.Editor.Classes.Bytecode.Locals;
using BCEdit180.Core.Editor.Classes.ExceptionTable;
using BCEdit180.Core.Editor.Classes.Methods;
using JavaAsm;

namespace BCEdit180.Core.Editor.Classes.Bytecode {
    public class CodeEditorViewModel : BaseViewModel {
        public BytecodeEditorViewModel ByteCodeEditor { get; }
        public ExceptionTableViewModel ExceptionEditor { get; }
        public LocalVariableTableViewModel LocalVariableTable { get; }
        public LocalVariableTypeTableViewModel LocalVariableTypeTable { get; }

        public MethodViewModel MethodInfo { get; }

        public CodeEditorViewModel(MethodViewModel methodInfo) {
            this.MethodInfo = methodInfo;
            this.ByteCodeEditor = new BytecodeEditorViewModel();
            this.ExceptionEditor = new ExceptionTableViewModel();
            this.LocalVariableTable = new LocalVariableTableViewModel();
            this.LocalVariableTypeTable = new LocalVariableTypeTableViewModel();
        }

        public void Load(MethodNode node) {
            this.ByteCodeEditor.Load(node);
            this.ExceptionEditor.Load(node);
            this.LocalVariableTable.Load(node);
            this.LocalVariableTypeTable.Load(node);
        }

        public void Save(MethodNode node) {
            this.ByteCodeEditor.Save(node);
            this.ExceptionEditor.Save(node);
            this.LocalVariableTable.Save(node);
            this.LocalVariableTypeTable.Save(node);
        }
    }
}