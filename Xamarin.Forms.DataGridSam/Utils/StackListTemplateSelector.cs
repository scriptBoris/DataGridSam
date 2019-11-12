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
            return Template;
        }
    }
}
