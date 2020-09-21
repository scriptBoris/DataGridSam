using System;
using System.Collections.Generic;
using System.Text;

namespace DataGridSam.Enums
{
    /// <summary>
    /// AutoNumber flag for anyone columns
    /// </summary>
    #if RELEASE
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    #endif
    public enum AutoNumberType
    {
        None,
        Up,
        Down,
    }

    /// <summary>
    /// Internal implementation system AutoNumber
    /// </summary>
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
