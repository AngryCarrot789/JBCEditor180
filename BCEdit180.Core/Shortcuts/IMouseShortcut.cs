using System.Collections.Generic;
using BCEdit180.Core.Shortcuts.Inputs;
using BCEdit180.Core.Shortcuts.Usage;

namespace BCEdit180.Core.Shortcuts {
    /// <summary>
    /// An interface for shortcuts that accept mouse inputs
    /// </summary>
    public interface IMouseShortcut : IShortcut {
        /// <summary>
        /// All of the Mouse Strokes that this shortcut contains
        /// </summary>
        IEnumerable<MouseStroke> MouseStrokes { get; }

        /// <summary>
        /// This can be used in order to track the usage of <see cref="IShortcut.InputStrokes"/>. If
        /// the list is empty, then the return value of this function is effectively pointless
        /// </summary>
        /// <returns></returns>
        IMouseShortcutUsage CreateMouseUsage();
    }
}