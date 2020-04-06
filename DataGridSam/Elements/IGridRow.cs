using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DataGridSam.Elements
{
    internal interface IGridRow
    {
        /// <summary>
        /// Host DataGrid
        /// </summary>
        DataGrid DataGrid { get; set; }

        /// <summary>
        /// Index row
        /// </summary>
        int Index { get; set; }

        /// <summary>
        /// Is selected row or not
        /// </summary>
        bool IsSelected { get; set; }

        object Context { get; set; }

        View SelectionBox { get; set; }

        View TouchBox { get; set; }
        
        View Line { get; set; }
        
        List<GridCell> Cells { get; set; }
        
        RowTrigger EnabledTrigger { get; set; }

        void UpdateStyle();
        void UpdateAutoNumeric(int num, int itemsCount);
        void UpdateLineVisibility(bool isVisible);
        void UpdateCellVisibility(int cellId, bool isVisible);
    }
}
