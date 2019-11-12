using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DataGridSam.Utils
{
    internal class StackListTemplateSelector : DataTemplateSelector
    {
        private static readonly DataTemplate Template = new DataTemplate(typeof(DataGridViewCell));

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            //var stack = container as StackList;
            //var par = stack.Parent?.Parent;
            //var dataGrid = par;

            //Template.SetValue(DataGridViewCell.DataGridProperty, dataGrid);
            Template.SetValue(DataGridViewCell.RowContextProperty, item);

            return Template;
        }
    }
}
