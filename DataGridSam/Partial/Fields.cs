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
        /// Main scroll. Contains: bodyGrid, mainStackLayout 
        /// (if enable pagination)
        /// </summary>
        internal ScrollView mainScroll;

        /// <summary>
        /// For imitate column borders (BODY)
        /// </summary>
        internal Grid maskGrid;

        /// <summary>
        /// For imitate column borders (HEAD)
        /// </summary>
        internal Grid maskHeadGrid;

        /// <summary>
        /// Main GUI component that display rows
        /// </summary>
        internal StackList stackList;

        /// <summary>
        /// Heads
        /// </summary>
        internal Grid headGrid;

        /// <summary>
        /// Contains: stackList, maskGrid
        /// </summary>
        internal Grid bodyGrid;

        /// <summary>
        /// Group of BoxViews imitate wrapper (for max FPS)
        /// </summary>
        internal BorderWrapper wrapper;



        /// <summary>
        /// Current time: selected row by user
        /// </summary>
        internal Row SelectedRow;



        /// <summary>
        /// Only for pagination
        /// </summary>
        internal int PaginationCurrentPageStartIndex;

        /// <summary>
        /// Only for pagination
        /// </summary>
        internal int PaginationCurrentPageItemsEndIndex;

        /// <summary>
        /// Only for pagination
        /// </summary>
        internal int PaginationCurrentPageEndIndex;

        /// <summary>
        /// For pagination
        /// </summary>
        internal Button buttonLatest;

        /// <summary>
        /// For pagination
        /// </summary>
        internal Button buttonNext;


        internal StyleContainer VisualRowsFromStyle = new StyleContainer();
        internal StyleContainer VisualRows = new StyleContainer();

        internal StyleContainer VisualSelectedRowFromStyle = new StyleContainer();
        internal StyleContainer VisualSelectedRow = new StyleContainer();
    }
}
