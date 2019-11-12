using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DataGridSam.Utils
{
    internal class StackListTemplateSelector : DataTemplateSelector
    {
        private static DataTemplate Template;

        public StackListTemplateSelector()
        {
            Template = new DataTemplate(typeof(DataGridViewCell));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var self = (StackList)container;

            //Binding context
            Template.SetValue(DataGridViewCell.DataGridProperty, self.DataGrid);
            Template.SetValue(DataGridViewCell.RowContextProperty, item);
            Template.SetValue(DataGridViewCell.BindingContextProperty, item);
            return Template;
        }
    }
}
