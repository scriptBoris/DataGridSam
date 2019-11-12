using System;
using System.Collections.Generic;
using System.Text;

namespace DataGridSam.Utils
{
    /// <summary>
    /// Для того, что бы избежать ошибки при установки биндинга
    /// </summary>
    public sealed class ColumnCollection : List<DataGridColumn>
    {
    }
}
