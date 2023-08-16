using System.Threading.Tasks;
using BCEdit180.Core.Shortcuts.Managing;

namespace BCEdit180.Shortcuts {
    public delegate Task<bool> ShortcutActivateHandler(ShortcutProcessor processor, GroupedShortcut shortcut);
}