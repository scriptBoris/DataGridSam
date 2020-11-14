using DataGridSam.Elements;
using DataGridSam.Enums;
using DataGridSam.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DataGridSam
{
    public partial class DataGrid
    {
        /// <summary>
        /// Absolute bottom line if DataGrid wrapped
        /// </summary>
        internal View absoluteBottom;

        /// <summary>
        /// Main scroll. Contains: bodyGrid, mainStackLayout 
        /// (if enable pagination)
        /// </summary>
        internal ScrollView mainScroll;

        /// <summary>
        /// Main GUI component that display rows
        /// </summary>
        internal StackList stackList;

        /// <summary>
        /// Heads
        /// </summary>
        internal GridHead headGrid;

        /// <summary>
        /// Contains: stackList, maskGrid
        /// </summary>
        internal GridBody bodyGrid;

        /// <summary>
        /// Current time: selected row by user
        /// </summary>
        internal GridRow SelectedRow;

        /// <summary>
        /// Columns based on position 
        /// </summary>
        internal List<DataGridColumn> InternalColumns = new List<DataGridColumn>();

        /// <summary>
        /// Need calc auto number?
        /// </summary>
        internal bool IsAutoNumberCalc => AutoNumberStrategy != AutoNumberStrategyType.None;

        /// <summary>
        /// Which way do you need to run the loop for automatic updates
        /// </summary>
        internal AutoNumberStrategyType AutoNumberStrategy = AutoNumberStrategyType.None;


        /// <summary>
        /// Visual container for ROWs from style
        /// </summary>
        internal VisualCollector VisualRowsFromStyle = new VisualCollector();
        /// <summary>
        /// Visual container for ROWs from DataGrid props
        /// </summary>
        internal VisualCollector VisualRows = new VisualCollector();
        /// <summary>
        /// Visual container for SELECTED ROW from style
        /// </summary>
        internal VisualCollector VisualSelectedRowFromStyle = new VisualCollector();
        /// <summary>
        /// Visual container for SELECTED ROW from DataGrid props
        /// </summary>
        internal VisualCollector VisualSelectedRow = new VisualCollector();

        /// <summary>
        /// Solve columns count for feature calculate 
        /// (dodge exceptions as ColumnSpan can not be zero)
        /// </summary>
        internal int ColumnSpan => (Columns?.Count > 0) ? Columns.Count : 1;
    }
}
