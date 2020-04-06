using DataGridSam.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace DataGridSam.Elements
{
    /// <summary>
    /// RepeatableStack
    /// StackLayout corresponding to ItemsSource and ItemTemplate
    /// </summary>
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    internal class StackList : StackLayout
    {
        internal int ItemsCount;

        // ItemsSource
        public static BindableProperty ItemsSourceProperty =
            BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(StackList), null, defaultBindingMode: BindingMode.OneWay,
                propertyChanged: ItemsChanged);
        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        // Has items
        public static BindableProperty HasItemsProperty =
            BindableProperty.Create(nameof(HasItems), typeof(bool), typeof(StackList), true);
        public bool HasItems
        {
            get { return (bool)GetValue(HasItemsProperty); }
            set { SetValue(HasItemsProperty, value); }
        }

        // DataGrid
        public static readonly BindableProperty DataGridProperty =
            BindableProperty.Create(nameof(DataGrid), typeof(DataGrid), typeof(StackList), null);
        public DataGrid DataGrid
        {
            get { return (DataGrid)GetValue(DataGridProperty); }
            set { SetValue(DataGridProperty, value); }
        }

        private static void ItemsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var self = (StackList)bindable;

            IEnumerable newList;
            try
            {
                newList = newValue as IEnumerable;
            }
            catch (Exception e)
            {
                self.ItemsCount = 0;
                throw e;
            }

            if (oldValue is INotifyCollectionChanged oldObservableCollection)
                oldObservableCollection.CollectionChanged -= self.OnItemsSourceCollectionChanged;

            if (newValue is INotifyCollectionChanged newObservableCollection)
                newObservableCollection.CollectionChanged += self.OnItemsSourceCollectionChanged;

            self.Children.Clear();

            // Detect items count 
            self.ItemsCount = 0;
            if (newValue is ICollection collection)
            {
                self.ItemsCount = collection.Count;
            }
            else if (newList != null)
            {
                var enumerator = newList.GetEnumerator();
                if (enumerator != null)
                    while (enumerator.MoveNext())
                        self.ItemsCount++;
            }

            if (newList != null)
            {
                int i = 0;
                IGridRow last = null;
                foreach (var item in newList)
                {
                    // Say triggers what binding context changed
                    if (i == 0)
                        self.DataGrid.OnChangeItemsBindingContext(item.GetType());

                    last = self.AddRow(item, i, self.ItemsCount);
                    i++;
                }

                if (last != null)
                    last.UpdateLineVisibility(false);
            }

            self.HasItems = (self.ItemsCount > 0);
            self.UpdateChildrenLayout();
            self.InvalidateLayout();
        }

        private void OnItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Replace
            if (e.Action == NotifyCollectionChangedAction.Replace)
            {
                int oldId = e.OldStartingIndex;
                int newId = e.NewStartingIndex;

                Children.RemoveAt(oldId);
                var row = InsertRow(e.NewItems[newId], newId, ItemsCount);

                // Hide line if row is last
                if (ItemsCount == oldId + 1)
                    row.UpdateLineVisibility(false);
            }
            // Add
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (e.NewItems == null)
                    return;

                // Common calc
                ItemsCount += e.NewItems.Count;
                bool isInsert = false;
                bool isAdd = false;
                if (Children.Count == 0)
                    isAdd = true;
                else if (e.NewStartingIndex < Children.Count)
                    isInsert = true;
                else
                    isAdd = true;

                IGridRow lastRow = null;

                // For add - previous line set visible
                if (isAdd)
                {
                    lastRow = Children.LastOrDefault() as IGridRow;
                    if (lastRow != null)
                    {
                        lastRow.UpdateLineVisibility(true);
                        lastRow = null;
                    }
                }

                int pause = 0;
                for (var i = 0; i < e.NewItems.Count; ++i)
                {
                    int index = i + e.NewStartingIndex;
                    pause = index;
                    var item = e.NewItems[i];

                    if (isInsert)
                    {
                        // Insert row
                        lastRow = InsertRow(item, index, ItemsCount);
                    }
                    else
                    {
                        // Add row
                        lastRow = AddRow(item, index, ItemsCount);
                    }
                }

                // Hide last row line
                if (lastRow != null && isAdd)
                    lastRow.UpdateLineVisibility(false);

                // recalc autonumber
                if (DataGrid.IsAutoNumberCalc)
                {
                    if (DataGrid.AutoNumberStrategy == Enums.AutoNumberStrategyType.Both)
                    {
                        for (int i = 0; i < Children.Count; i++)
                            ((IGridRow)Children[i]).UpdateAutoNumeric(i + 1, ItemsCount);
                    }
                    else if (DataGrid.AutoNumberStrategy == Enums.AutoNumberStrategyType.Down)
                    {
                        for (int i = pause; i < Children.Count; i++)
                            ((IGridRow)Children[i]).UpdateAutoNumeric(i + 1, ItemsCount);
                    }
                    else if (DataGrid.AutoNumberStrategy == Enums.AutoNumberStrategyType.Up)
                    {
                        for (int i = pause; i >= 0; i--)
                            ((IGridRow)Children[i]).UpdateAutoNumeric(i + 1, ItemsCount);
                    }
                }
            }
            // Remove
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                if (e.OldItems == null)
                    return;

                // Common calc
                ItemsCount -= e.OldItems.Count;

                int delIndex = e.OldStartingIndex;
                bool isLast = (delIndex == ItemsCount);
                Children.RemoveAt(delIndex);

                if (isLast)
                {
                    var last = Children.LastOrDefault() as IGridRow;
                    if (last != null)
                    {
                        last.UpdateLineVisibility(false);
                    }
                }
                    
                if (DataGrid.IsAutoNumberCalc)
                {
                    // recalc autonumber
                    if (DataGrid.AutoNumberStrategy == Enums.AutoNumberStrategyType.Both)
                    {
                        for (int i = 0; i < Children.Count; i++)
                            (Children[i] as IGridRow).UpdateAutoNumeric(i + 1, ItemsCount);
                    }
                    else if (DataGrid.AutoNumberStrategy == Enums.AutoNumberStrategyType.Down)
                    {
                        for (int i = delIndex; i < Children.Count; i++)
                            (Children[i] as IGridRow).UpdateAutoNumeric(i + 1, ItemsCount);
                    }
                    else if (DataGrid.AutoNumberStrategy == Enums.AutoNumberStrategyType.Up)
                    {
                        if (ItemsCount > 0)
                        {
                            delIndex--;
                            if (delIndex < 0)
                                delIndex = 0;

                            for (int i = delIndex; i >= 0; i--)
                                (Children[i] as IGridRow).UpdateAutoNumeric(i + 1, ItemsCount);
                        }
                    }
                }
            }
            // Reset
            else if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                Children.Clear();
                ItemsCount = 0;
            }
            else
            {
                return;
            }

            HasItems = (ItemsCount > 0);
        }

        /// <summary>
        /// Создает строку для таблицы
        /// </summary>
        /// <param name="bindItem">Модель данных</param>
        /// <param name="host">Корневой элемент</param>        
        /// <param name="index">Индекс элемента таблицы</param>
        /// <param name="itemsCount">Количество элементов таблицы</param>
        private IGridRow AddRow(object bindItem, int index, int itemsCount)
        {
            var type = RowImplementations.CustomGrid;
            IGridRow row = null;

            if (type == RowImplementations.StackLine)
            {
                row = new GridRow(bindItem, DataGrid, index, itemsCount);
            }
            else if (type == RowImplementations.CustomGrid)
            {
                row = new RowCustom(bindItem, DataGrid, index, itemsCount);
            }

            Children.Add(row as View);
            return row;
        }

        private IGridRow InsertRow(object bindItem, int index, int itemsCount)
        {
            var type = RowImplementations.CustomGrid;
            IGridRow row = null;

            if (type == RowImplementations.StackLine)
            {
                row = new GridRow(bindItem, DataGrid, index, itemsCount);
            }
            else if (type == RowImplementations.CustomGrid)
            {
                row = new RowCustom(bindItem, DataGrid, index, itemsCount);
            }

            Children.Insert(index, row as View);
            return row;
        }
    }
}