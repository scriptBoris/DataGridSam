using DataGridSam.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataGridSam
{
    public partial class DataGrid
    {
        /// <summary>
        /// For INTERNAL USING
        /// Current time: selected cell by user
        /// </summary>
        internal Row SelectedRow;

        /// <summary>
        /// For INTERNAL USING
        /// Can block event property changed for property 
        /// "SelectedItem"
        /// </summary>
        internal bool blockThrowPropChanged;

        /// <summary>
        /// For INTERNAL USING
        /// </summary>
        internal int PaginationCurrentPageStartIndex;

        /// <summary>
        /// For INTERNAL USING
        /// </summary>
        internal int PaginationCurrentPageItemsEndIndex;


        /// <summary>
        /// For INTERNAL USING
        /// </summary>
        internal int PaginationCurrentPageEndIndex;
    }
}
