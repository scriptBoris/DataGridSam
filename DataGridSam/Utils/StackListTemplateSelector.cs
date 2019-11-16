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
            Template = new DataTemplate(typeof(Row));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var self = (StackList)container;

            // Create row
            Template.SetValue(Row.DataGridProperty, self.DataGrid);
            
            // Passing value context
            Template.SetValue(Row.BindingContextProperty, item);

            //Template.SetValue(Row.RowContextProperty, item);

            return Template;
        }
    }
}
