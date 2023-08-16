using BCEdit180.Core.Editor.Classes.Bytecode.Instructions;

namespace BCEdit180.Core.Editor.Classes.Bytecode {
    public interface ILabelTargeter {
        LabelViewModel TargetLabel { get; set; }
    }
}