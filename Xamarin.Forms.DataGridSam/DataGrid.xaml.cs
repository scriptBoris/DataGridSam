using DataGridSam.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DataGridSam
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DataGrid : Grid
    {
		//public event EventHandler Refreshing;

        public DataGrid()
        {
            InitializeComponent();

            stackList.ItemTemplate = new StackListTemplateSelector();
        }

        // Columns
        public static readonly BindableProperty ColumnsProperty =
            BindableProperty.Create(nameof(Columns), typeof(ColumnCollection), typeof(DataGrid),
                propertyChanged: (bindableObj, o, n) => 
                {
                    (bindableObj as DataGrid).InitHeaderView();
                },
                defaultValueCreator: b => 
                {
                    return new ColumnCollection(); 
                }
            );
        public ColumnCollection Columns
        {
            get { return (ColumnCollection)GetValue(ColumnsProperty); }
            set { SetValue(ColumnsProperty, value); }
        }


        // Items source
        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(DataGrid), null,
                propertyChanged: (thisObject, oldValue, newValue) =>
                {
                    DataGrid self = thisObject as DataGrid;

                    self.stackList.ItemsSource = newValue as IEnumerable;
                });
        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        // Command selected item 
        public static readonly BindableProperty CommandSelectedItemProperty = 
            BindableProperty.Create(nameof(CommandSelectedItem), typeof(ICommand), typeof(DataGrid), null, BindingMode.TwoWay);

        public ICommand CommandSelectedItem
        {
            get { return (ICommand)GetValue(CommandSelectedItemProperty); }
            set { SetValue(CommandSelectedItemProperty, value); }
        }

        // Lines width
        public static readonly BindableProperty LinesWidthProperty =
            BindableProperty.Create(nameof(LinesWidth), typeof(double), typeof(DataGrid), 2.0, BindingMode.Default);

        public double LinesWidth
        {
            get { return (double)GetValue(LinesWidthProperty); }
            set { SetValue(LinesWidthProperty, value); }
        }
    }
}