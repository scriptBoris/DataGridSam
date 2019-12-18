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
        internal ScrollView scroll;

        /// <summary>
        /// For imitate columns lines
        /// </summary>
        internal Grid maskGrid;

        /// <summary>
        /// Main GUI component that display rows
        /// </summary>
        internal StackList stackList;

        /// <summary>
        /// Heads
        /// </summary>
        internal Grid headGrid;

        /// <summary>
        /// Contains: maskGrid, stackList
        /// </summary>
        internal Grid bodyGrid;




        /// <summary>
        /// Current time: selected row by user
        /// </summary>
        internal Row SelectedRow;

        /// <summary>
        /// Can block event property changed for property 
        /// "SelectedItem"
        /// </summary>
        internal bool isBlockThrowPropChanged;



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
    }
}
