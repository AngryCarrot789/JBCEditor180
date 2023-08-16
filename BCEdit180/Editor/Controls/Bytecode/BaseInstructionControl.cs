using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BCEdit180.Core.Editor.Classes.Bytecode.Instructions;

namespace BCEdit180.Editor.Controls.Bytecode {
    public class BaseInstructionControl : Control {
        public static readonly DependencyProperty OpcodeTextBrushProperty =
            DependencyProperty.Register(
                "OpcodeTextBrush",
                typeof(Brush),
                typeof(BaseInstructionControl),
                new PropertyMetadata(new SolidColorBrush(Colors.Orange)));

        [Category("Brush")]
        public Brush OpcodeTextBrush {
            get => (Brush) this.GetValue(OpcodeTextBrushProperty);
            set => this.SetValue(OpcodeTextBrushProperty, value);
        }

        public BaseInstructionViewModel ViewModel {
            get => (BaseInstructionViewModel) this.DataContext;
        }

        public BaseInstructionControl() {

        }
    }
}