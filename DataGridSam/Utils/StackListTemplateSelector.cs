using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DataGridSam.Utils
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    internal class StackListTemplateSelector : DataTemplateSelector
    {
        private static DataTemplate Template;

        public StackListTemplateSelector()
        {
            Template = new DataTemplate(typeof(StackCell));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var self = (StackList)container;

            //Binding context
            Template.SetValue(StackCell.DataGridProperty, self.DataGrid);
            Template.SetValue(StackCell.RowContextProperty, item);
            Template.SetValue(StackCell.BindingContextProperty, item);
            return Template;
        }
    }
}
