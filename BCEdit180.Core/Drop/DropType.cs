using System;

namespace BCEdit180.Core.Interactivity {
    [Flags]
    public enum DropType {
        None,
        Copy,
        Move,
        All = Copy | Move
    }
}