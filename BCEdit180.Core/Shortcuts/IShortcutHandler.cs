using System.Threading.Tasks;
using BCEdit180.Core.Shortcuts.Managing;

namespace BCEdit180.Core.Shortcuts {
    public interface IShortcutHandler {
        Task<bool> OnShortcutActivated(ShortcutProcessor processor, GroupedShortcut shortcut);
    }
}