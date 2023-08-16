using BCEdit180.Core;
using BCEdit180.Core.Shortcuts.Dialogs;
using BCEdit180.Core.Shortcuts.Inputs;

namespace BCEdit180.Shortcuts.Dialogs {
    [ServiceImplementation(typeof(IKeyboardDialogService))]
    public class KeyboardDialogService : IKeyboardDialogService {
        public KeyStroke? ShowGetKeyStrokeDialog() {
            KeyStrokeInputWindow window = new KeyStrokeInputWindow();
            if (window.ShowDialog() != true || window.Stroke.Equals(default)) {
                return null;
            }
            else {
                return window.Stroke;
            }
        }
    }
}