using System.Windows.Input;

namespace BCEdit180.Utils {
    public static class KBUtils {
        public static bool AreModsPressed(ModifierKeys keys) {
            return (Keyboard.Modifiers & keys) == keys;
        }
    }
}