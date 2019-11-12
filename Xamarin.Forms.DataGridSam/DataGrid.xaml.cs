using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DataGridSam
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DataGrid : Grid
    {
        public DataGrid()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty ColumnsProperty =
            BindableProperty.Create(nameof(Columns), typeof(List<DataGridColumn>), typeof(DataGrid),
                propertyChanged: (bindableObj, o, n) => 
                {
                    (bindableObj as DataGrid).InitHeaderView();
                },
                defaultValueCreator: b => 
                {
                    return new List<DataGridColumn>(); 
                }
            );


        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(DataGrid), null,
                propertyChanged: (thisObject, oldValue, newValue) => 
                {
                    DataGrid self = thisObject as DataGrid;

                    self.stackList.ItemsSource = newValue as IEnumerable;

                    // ObservableCollection Tracking 
                    //if (oldValue != null && oldValue is INotifyCollectionChanged prevList)
                    //{
                    //    //prevList.CollectionChanged -= self.HandleItemsSourceCollectionChanged;
                    //}

                    //// Привязка
                    //if (newValue != null && newValue is INotifyCollectionChanged nextList)
                    //{
                    //    nextList.CollectionChanged += self.HandleItemsSourceCollectionChanged;
                    //}

                    //if (self.SelectedItem != null && !self.InternalItems.Contains(self.SelectedItem))
                    //    self.SelectedItem = null;

                    //if (self.NoDataView != null)
                    //{
                    //    if (self.ItemsSource == null || self.InternalItems.Count() == 0)
                    //        self._noDataView.IsVisible = true;
                    //    else if (self._noDataView.IsVisible)
                    //        self._noDataView.IsVisible = false;
                    //}
                });

        public List<DataGridColumn> Columns
        {
            get { return (List<DataGridColumn>)GetValue(ColumnsProperty); }
            set { SetValue(ColumnsProperty, value); }
        }

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
    }
}