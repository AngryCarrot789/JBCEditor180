using System;

namespace BCEdit180.Core.Drop {
    [Flags]
    public enum DropType {
        None = 0,
        Copy = 1,
        Move = 2,
        Link = 4,
        Scroll = -2147483648, // 0x80000000
        All = Scroll | Move | Copy, // 0x80000003
    }
}