using System;
using System.Collections.Generic;
using System.Text;

namespace DataGridSam.Utils
{
    /// <summary>
    /// Для того, что бы избежать ошибки при установки биндинга
    /// </summary>
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public sealed class ColumnCollection : List<DataGridColumn>
    {
    }
}
