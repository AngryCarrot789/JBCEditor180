using System.Threading.Tasks;
using BCEdit180.Core.Views.Dialogs.Message;

namespace BCEdit180.Core.Utils {
    public static class ClipboardUtils {
        public static async Task<bool> SetClipboardOrShowErrorDialog(string text) {
            if (IoC.Clipboard == null) {
                await Dialogs.ClipboardUnavailableDialog.ShowAsync("No clipboard", "Clipboard is unavailable.\n" + text);
                return false;
            }
            else {
                if (!IoC.Clipboard.SetText(text))
                    await Dialogs.ClipboardUnavailableDialog.ShowAsync("Clipboard Failure", "Failed to set the clipboard to:\n" + text);
                return true;
            }
        }
    }
}