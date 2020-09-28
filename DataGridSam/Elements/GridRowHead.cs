using DataGridSam.Platform;
using DataGridSam.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
//using TouchSam;
//using DataGridSam.Platform;
using Xamarin.Forms;

namespace DataGridSam.Elements
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    internal sealed class GridRowHead : GridRowBase<GridCellHead>
    {
        public GridRowHead(DataGrid host) : base(host.BindingContext, host, 0, host.HeaderHasBorder)
        {
        }

        protected override void RedrawElements(object context)
        {
        }
    }
}
