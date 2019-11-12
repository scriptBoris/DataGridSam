using System;
using System.Collections.Generic;
using System.Text;

namespace DataGridSam
{
    public partial class DataGrid
    {
        protected override void OnParentSet()
        {
            base.OnParentSet();
            InitHeaderView();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            SetColumnsBindingContext();
        }
    }
}
