using System;
using System.Collections.Generic;
using System.Text;

namespace DataGridSam.Enums
{
    #if RELEASE
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    #endif
    public enum AutoNumberType
    {
        None,
        Up,
        Down,
    }

    #if RELEASE
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    #endif
    internal enum AutoNumberStrategyType
    {
        None,
        Up,
        Down,
        Both,
    }
}
