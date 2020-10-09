using Sample.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Sample.Views
{
    public partial class DevView : ContentPage
    {
        public DevView()
        {
            InitializeComponent();
        }

        public void ScrollToIndex(int id)
        {
            dataGrid.ScrollToElement(id, ScrollToPosition.End, true);
        }
    }
}
