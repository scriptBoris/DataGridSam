using DataGridSam.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DataGridSam.Elements
{
    internal class GridCellHead : GridCellBase
    {
        protected override void BuildContent()
        {
            Label = new Label();
            Label.Text = Column.Title;
        }
    }
}
