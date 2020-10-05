using DataGridSam.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.SymbolStore;
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
    internal class StackList : Layout<GridRow>
    {
        internal int ItemsCount;

        // ItemsSource
        public static BindableProperty ItemsSourceProperty =
            BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(StackList), null, defaultBindingMode: BindingMode.OneWay,
                propertyChanged: ItemsSourceChanged);
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

        private static void ItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var self = (StackList)bindable;
            self.ItemsCount = 0;
            self.Children.Clear();

            IEnumerable newList;
            try
            {
                newList = newValue as IEnumerable;
            }
            catch (Exception e)
            {
                self.DataGrid.UpdateEmptyViewVisible();
                throw e;
            }

            if (oldValue is INotifyCollectionChanged oldObservableCollection)
                oldObservableCollection.CollectionChanged -= self.OnItemsSourceCollectionChanged;

            if (newValue is INotifyCollectionChanged newObservableCollection)
                newObservableCollection.CollectionChanged += self.OnItemsSourceCollectionChanged;

            if (newList != null)
            {
                // Say triggers what binding context changed
                var targetType = newList.GetType().GetGenericArguments()[0];
                self.DataGrid.OnChangeItemsBindingContext(targetType);

                // Instantly add items
                int i = 0;
                object item = null;
                var enumerator = newList.GetEnumerator();
                if (enumerator.MoveNext())
                {
                    while (true)
                    {
                        item = enumerator.Current;

                        if (enumerator.MoveNext())
                        {
                            self.AddRow(item, i, true, false);
                            i++;
                        }
                        else
                        {
                            self.AddRow(item, i, false, false);
                            break;
                        }
                    }
                }

                // Update auto number
                if (self.DataGrid.AutoNumberStrategy != AutoNumberStrategyType.None)
                    foreach (var row in self.Children)
                        row.UpdateAutoNumeric(row.Index);
            }

            self.HasItems = (self.ItemsCount > 0);
            self.DataGrid.UpdateEmptyViewVisible();
        }

        private void OnItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Replace
            if (e.Action == NotifyCollectionChangedAction.Replace)
            {
                int oldId = e.OldStartingIndex;
                int newId = e.NewStartingIndex;
                bool isLast = (ItemsCount == oldId + 1);

                Children.RemoveAt(oldId);
                InsertRow(e.NewItems[newId], newId, isLast);
            }
            // Add or Insert
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (e.NewItems == null)
                    return;

                bool isInsert = (Children.Count > 0 && e.NewStartingIndex < Children.Count);

                // For add - previous line set visible
                if (!isInsert)
                {
                    var lastRow = Children.LastOrDefault() as GridRow;
                    if (lastRow != null)
                        lastRow.UpdateLineVisibility(true);
                }

                int index;
                // For each elements except the last
                for (var i = 0; i < e.NewItems.Count - 1; ++i)
                {
                    index = i + e.NewStartingIndex;
                    var item = e.NewItems[i];

                    if (isInsert)
                    {
                        // Insert row
                        InsertRow(item, index, true);
                    }
                    else
                    {
                        // Add row
                        AddRow(item, index, true);
                    }
                }

                index = e.NewStartingIndex + e.NewItems.Count - 1;
                
                // Last item from NewItems
                var lastItem = e.NewItems[e.NewItems.Count - 1];
                if (isInsert)
                    InsertRow(lastItem, index, !(index == ItemsCount) );
                else
                    AddRow(lastItem, index, !(index == ItemsCount) );

                // recalc autonumber
                if (DataGrid.IsAutoNumberCalc)
                {
                    if (DataGrid.AutoNumberStrategy == Enums.AutoNumberStrategyType.Both)
                    {
                        for (int i = 0; i < Children.Count; i++)
                            Children[i].UpdateAutoNumeric(i);
                    }
                    else if (DataGrid.AutoNumberStrategy == Enums.AutoNumberStrategyType.Down)
                    {
                        for (int i = index; i < Children.Count; i++)
                            Children[i].UpdateAutoNumeric(i);
                    }
                    else if (DataGrid.AutoNumberStrategy == Enums.AutoNumberStrategyType.Up)
                    {
                        for (int i = index; i >= 0; i--)
                            Children[i].UpdateAutoNumeric(i);
                    }
                }

                DataGrid.UpdateEmptyViewVisible();
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
                    var last = Children.LastOrDefault() as GridRow;
                    if (last != null)
                        last.UpdateLineVisibility(false);
                }
                    
                if (DataGrid.IsAutoNumberCalc)
                {
                    // recalc autonumber
                    if (DataGrid.AutoNumberStrategy == Enums.AutoNumberStrategyType.Both)
                    {
                        for (int i = 0; i < Children.Count; i++)
                            (Children[i] as GridRow).UpdateAutoNumeric(i);
                    }
                    else if (DataGrid.AutoNumberStrategy == Enums.AutoNumberStrategyType.Down)
                    {
                        for (int i = delIndex; i < Children.Count; i++)
                            (Children[i] as GridRow).UpdateAutoNumeric(i);
                    }
                    else if (DataGrid.AutoNumberStrategy == Enums.AutoNumberStrategyType.Up)
                    {
                        if (ItemsCount > 0)
                        {
                            delIndex--;
                            if (delIndex < 0)
                                delIndex = 0;

                            for (int i = delIndex; i >= 0; i--)
                                (Children[i] as GridRow).UpdateAutoNumeric(i);
                        }
                    }
                }

                DataGrid.UpdateEmptyViewVisible();
            }
            // Reset
            else if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                Children.Clear();
                ItemsCount = 0;
                DataGrid.UpdateEmptyViewVisible();
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
        private GridRow AddRow(object bindItem, int index, bool isLineVisible, bool isAutoNumber = true)
        {
            ItemsCount++;
            var row = new GridRow(bindItem, this, index, isLineVisible, isAutoNumber);
            Children.Add(row);
            return row;
        }

        private GridRow InsertRow(object bindItem, int index, bool isLineVisible)
        {
            ItemsCount++;
            var row = new GridRow(bindItem, this, index, isLineVisible, true);
            Children.Insert(index, row);
            return row;
        }

        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            if (Children.Count == 0)
                return new SizeRequest(new Size(widthConstraint, 0));
            else
            {
                double heightMeasure = 0;
                foreach (var item in Children)
                {
                    var res = item.Measure(widthConstraint, double.PositiveInfinity, MeasureFlags.IncludeMargins);
                    heightMeasure += res.Request.Height;
                }

                return new SizeRequest(new Size(widthConstraint, heightMeasure));
            }
        }

        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            if (Children.Count == 0)
                return;

            double offsetY = 0;
            for (int i = 0; i < Children.Count; i++)
            {
                var row = Children[i];
                double rowHeight = row.RowHeight;
                var rect = new Rectangle(0, offsetY, width, rowHeight);

                // Draw row
                row.RenderRow(0, 0, width, rowHeight);
                // And draw row? O_O (joke) This thing is needed to fix 
                // bug with empty rows after Foreach=>Itemsource.Add(item)
                LayoutChildIntoBoundingRegion(row, rect);

                offsetY += rowHeight;
            }
        }
    }
}